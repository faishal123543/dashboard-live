using LoanDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StagesController : ControllerBase
{
    private readonly IStageService _stageService;
    private readonly IPartnerService _partnerService;

    public StagesController(IStageService stageService, IPartnerService partnerService)
    {
        _stageService   = stageService;
        _partnerService = partnerService;
    }

    /// <summary>
    /// Returns today's stage counts pulled live from SQL Server.
    /// Optional <c>partnerId</c> query parameter filters counts to a single partner;
    /// omit or pass 0 for the all-partners view.
    /// </summary>
    [HttpGet("count")]
    public async Task<IActionResult> GetStageCounts([FromQuery] int? partnerId, CancellationToken ct)
        => Ok(await _stageService.GetStageCountsAsync(partnerId, ct));

    /// <summary>Returns the active partner list used by the dropdown.</summary>
    [HttpGet("partners")]
    public async Task<IActionResult> GetPartners(CancellationToken ct)
        => Ok(await _partnerService.GetPartnersAsync(ct));
}
