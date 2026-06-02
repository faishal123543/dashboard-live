using LoanDashboard.Models;

namespace LoanDashboard.Services;

public interface IStageService
{
    Task<IReadOnlyList<StageCountDto>> GetStageCountsAsync(CancellationToken ct = default);
}
