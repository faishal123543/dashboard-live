using Microsoft.AspNetCore.SignalR;

namespace LoanDashboard.Hubs;

public class LoanDashboardHub : Hub
{
    private readonly ILogger<LoanDashboardHub> _logger;

    public LoanDashboardHub(ILogger<LoanDashboardHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "dashboard");
        _logger.LogInformation("Client connected: {Id}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "dashboard");
        _logger.LogInformation("Client disconnected: {Id}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
