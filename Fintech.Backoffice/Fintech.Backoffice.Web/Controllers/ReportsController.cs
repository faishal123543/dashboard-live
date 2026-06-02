using Microsoft.AspNetCore.Mvc;

namespace Fintech.Backoffice.Web.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetReportData()
        {
            return Json(new
            {
                // Monthly approved amounts (last 6 months)
                MonthlyAmounts = new
                {
                    Labels = new[] { "Dec", "Jan", "Feb", "Mar", "Apr", "May" },
                    Values = new[] { 2400000, 2800000, 3200000, 2950000, 3450000, 3650000 }
                },
                // Status distribution
                StatusBreakdown = new
                {
                    Labels = new[] { "Completed", "In-Progress", "Rejected", "Cancelled" },
                    Values = new[] { 1042, 23, 156, 26 },
                    Colors = new[] { "#00ff88", "#00d4ff", "#ff4757", "#ffaa00" }
                },
                // Top performing partners
                TopPartners = new
                {
                    Labels = new[] { "Emirates Bank", "First National", "Gulf Credit", "FinTech Sol.", "Digital Loans" },
                    Values = new[] { 320, 285, 234, 198, 210 }
                },
                // Application growth trend
                GrowthTrend = new
                {
                    Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" },
                    Submitted = new[] { 145, 178, 195, 210, 245, 280 },
                    Approved = new[] { 98, 125, 145, 165, 195, 215 }
                }
            });
        }
    }
}
