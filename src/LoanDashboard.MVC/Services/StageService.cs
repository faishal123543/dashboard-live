using Dapper;
using LoanDashboard.Models;
using Microsoft.Data.SqlClient;

namespace LoanDashboard.Services;

/// <summary>
/// Stage service with TWO sources working together:
///
///   1. <b>EP101_StageEffective</b> (database)
///       → tells us the LIVE catalog of stages and their real StageNo values
///       → so when a stage's name changes or its StageNo gets reassigned,
///         the dashboard picks it up automatically.
///
///   2. <b>KnownStages</b> (appsettings.json)
///       → tells us WHICH stages are business-approved for the dashboard.
///       → guards against random workflow stages (e.g. internal-only stages,
///         test stages) accidentally appearing in production.
///
/// To add a NEW stage to the dashboard in production, just add its name to
/// <c>KnownStages</c> in appsettings.json — no code change, no redeploy
/// beyond a config refresh.
/// </summary>
public class StageService : IStageService
{
    private readonly string _connectionString;
    private readonly string[] _visibleStages;
    private readonly ILogger<StageService> _logger;

    /// <summary>
    /// Returns the entire stage catalog (latest workflow) joined with today's
    /// completion counts. C# filters to <c>KnownStages</c> afterward.
    /// </summary>
    private const string StageCountSql = @"
        ;WITH stage_master AS (
            SELECT  StageNo, StageName_E
            FROM    dbo.EP101_StageEffective WITH (NOLOCK)
            WHERE   WFEffective_Id = (
                        SELECT MAX(WFEffective_Id)
                        FROM   dbo.EP101_StageEffective WITH (NOLOCK)
                    )
        ),
        today_counts AS (
            SELECT  ps.StageNo, COUNT(ps.ProcessNo) AS cnt
            FROM    dbo.EP101_Process            p   WITH (NOLOCK)
            LEFT JOIN dbo.EP101_ProcessStage     ps  WITH (NOLOCK)
                   ON ps.Process_Id      = p.Id
            WHERE   ps.IsApplicable = 1
              AND   ps.CompletedOn IS NOT NULL
              AND   ps.CreatedOn  >= CAST(GETDATE() AS DATE)
              AND   ps.CreatedOn  <  DATEADD(day, 1, CAST(GETDATE() AS DATE))
            GROUP BY ps.StageNo
        )
        SELECT  sm.StageNo                            AS StageNo,
                sm.StageName_E                        AS StageName,
                COALESCE(tc.cnt, 0)                   AS [Count]
        FROM    stage_master sm
        LEFT JOIN today_counts tc ON tc.StageNo = sm.StageNo
        ORDER BY sm.StageNo;";

    public StageService(IConfiguration cfg, ILogger<StageService> logger)
    {
        _connectionString = cfg.GetConnectionString("LoanPortal")
            ?? throw new InvalidOperationException("Missing connection string 'LoanPortal'.");
        _visibleStages = cfg.GetSection("KnownStages").Get<string[]>() ?? Array.Empty<string>();
        _logger        = logger;
    }

    public async Task<IReadOnlyList<StageCountDto>> GetStageCountsAsync(CancellationToken ct = default)
    {
        // 1) Pull the full DB picture
        List<StageCountDto> dbRows;
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            var rows = await conn.QueryAsync<StageCountDto>(
                new CommandDefinition(StageCountSql, commandTimeout: 30, cancellationToken: ct));
            dbRows = rows.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch stage counts");
            dbRows = new List<StageCountDto>();
        }

        var now = DateTime.UtcNow;

        // 2) If no whitelist is configured, return everything from the DB as-is
        if (_visibleStages.Length == 0)
        {
            foreach (var r in dbRows) r.LastUpdated = now;
            return dbRows;
        }

        // 3) Whitelist applied — keep only stages business-approved for the dashboard.
        //    Stages in KnownStages but missing from DB are still shown (count = 0)
        //    so the operator always sees a complete pipeline view.
        var byName = dbRows.ToDictionary(r => r.StageName, r => r,
                                         StringComparer.OrdinalIgnoreCase);

        var result = new List<StageCountDto>(_visibleStages.Length);
        var fallbackNo = 9000;

        foreach (var name in _visibleStages)
        {
            if (byName.TryGetValue(name, out var found))
            {
                found.LastUpdated = now;
                result.Add(found);
            }
            else
            {
                // Configured stage not yet present in EP101_StageEffective —
                // show it with zero so missing master-data is obvious.
                result.Add(new StageCountDto
                {
                    StageNo     = fallbackNo++,
                    StageName   = name,
                    Count       = 0,
                    LastUpdated = now
                });
            }
        }

        return result
            .OrderBy(r => r.StageNo)
            .ToList();
    }
}
