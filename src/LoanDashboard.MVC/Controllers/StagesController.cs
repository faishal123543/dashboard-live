using LoanDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StagesController : ControllerBase
{
    private readonly IStageService _service;

    public StagesController(IStageService service)
    {
        _service = service;
    }

    /// <summary>Returns today's stage counts pulled live from SQL Server.</summary>
    [HttpGet("count")]
    public async Task<IActionResult> GetStageCounts(CancellationToken ct)
        => Ok(await _service.GetStageCountsAsync(ct));
}
