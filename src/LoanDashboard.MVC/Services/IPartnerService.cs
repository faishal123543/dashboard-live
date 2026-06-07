using LoanDashboard.Models;

namespace LoanDashboard.Services;

public interface IPartnerService
{
    Task<IReadOnlyList<PartnerDto>> GetPartnersAsync(CancellationToken ct = default);
}
