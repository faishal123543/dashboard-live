using LoanDashboard.Models;

namespace LoanDashboard.Services;

public interface IStageService
{
    /// <summary>
    /// Returns today's stage counts. When <paramref name="partnerId"/> is supplied,
    /// only processes belonging to that partner are counted; pass null for the
    /// all-partners view.
    /// </summary>
    Task<IReadOnlyList<StageCountDto>> GetStageCountsAsync(int? partnerId = null, CancellationToken ct = default);
}
