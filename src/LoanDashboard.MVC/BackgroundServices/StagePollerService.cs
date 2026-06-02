using LoanDashboard.Hubs;
using LoanDashboard.Models;
using LoanDashboard.Services;
using Microsoft.AspNetCore.SignalR;

namespace LoanDashboard.BackgroundServices;

/// <summary>
/// Polls SQL Server on a fixed interval and pushes daily stage counts to all
/// connected SignalR clients. Only broadcasts when counts actually change.
/// </summary>
public class StagePollerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<LoanDashboardHub> _hub;
    private readonly ILogger<StagePollerService> _logger;
    private readonly TimeSpan _interval;
    private string? _lastHash;

    public StagePollerService(
        IServiceScopeFactory scopeFactory,
        IHubContext<LoanDashboardHub> hub,
        ILogger<StagePollerService> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _hub = hub;
        _logger = logger;
        _interval = TimeSpan.FromSeconds(
            configuration.GetValue("Poller:IntervalSeconds", 10));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StagePollerService started — polling every {Sec}s", _interval.TotalSeconds);

        await PollOnceAsync(stoppingToken);

        using var timer = new PeriodicTimer(_interval);
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
                await PollOnceAsync(stoppingToken);
        }
        catch (OperationCanceledException) { /* graceful shutdown */ }
    }

    private async Task PollOnceAsync(CancellationToken ct)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStageService>();

            var counts = await service.GetStageCountsAsync(ct);

            var hash = ComputeHash(counts);
            if (hash == _lastHash) return;
            _lastHash = hash;

            await _hub.Clients.Group("dashboard").SendAsync("ReceiveStageCounts", counts, ct);
            _logger.LogDebug("Broadcast {Stages} stage counts", counts.Count);
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Poll cycle failed — will retry in {Sec}s", _interval.TotalSeconds);
        }
    }

    private static string ComputeHash(IReadOnlyList<StageCountDto> counts) =>
        string.Join("|", counts.Select(c => $"{c.StageNo}:{c.Count}"));
}
