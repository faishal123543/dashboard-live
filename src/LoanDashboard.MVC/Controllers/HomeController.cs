using LoanDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanDashboard.Controllers;

public class HomeController : Controller
{
    private readonly IStageService _stageService;
    private readonly IPartnerService _partnerService;

    public HomeController(IStageService stageService, IPartnerService partnerService)
    {
        _stageService   = stageService;
        _partnerService = partnerService;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var stagesTask   = _stageService.GetStageCountsAsync(partnerId: null, ct);
        var partnersTask = _partnerService.GetPartnersAsync(ct);
        await Task.WhenAll(stagesTask, partnersTask);

        ViewData["Partners"] = await partnersTask;
        return View(await stagesTask);
    }

    [Route("Home/Error")]
    public IActionResult Error() => View();
}
