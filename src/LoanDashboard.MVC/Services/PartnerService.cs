using Dapper;
using LoanDashboard.Models;
using Microsoft.Data.SqlClient;

namespace LoanDashboard.Services;

public class PartnerService : IPartnerService
{
    private readonly string _connectionString;
    private readonly ILogger<PartnerService> _logger;

    private const string PartnerSql = @"
        SELECT  PartnerId, PartnerName
        FROM    dbo.EP101_Partner WITH (NOLOCK)
        WHERE   IsActive = 1
        ORDER BY PartnerName;";

    public PartnerService(IConfiguration cfg, ILogger<PartnerService> logger)
    {
        _connectionString = cfg.GetConnectionString("LoanPortal")
            ?? throw new InvalidOperationException("Missing connection string 'LoanPortal'.");
        _logger = logger;
    }

    public async Task<IReadOnlyList<PartnerDto>> GetPartnersAsync(CancellationToken ct = default)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            var rows = await conn.QueryAsync<PartnerDto>(
                new CommandDefinition(PartnerSql, commandTimeout: 30, cancellationToken: ct));
            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch partner list");
            return Array.Empty<PartnerDto>();
        }
    }
}
