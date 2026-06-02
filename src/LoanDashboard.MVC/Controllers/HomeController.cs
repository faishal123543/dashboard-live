using LoanDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanDashboard.Controllers;

public class HomeController : Controller
{
    private readonly IStageService _service;

    public HomeController(IStageService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var stages = await _service.GetStageCountsAsync(ct);
        return View(stages);
    }

    [Route("Home/Error")]
    public IActionResult Error() => View();
}
