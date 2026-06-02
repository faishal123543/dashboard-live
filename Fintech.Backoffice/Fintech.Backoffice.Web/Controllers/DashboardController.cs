using Microsoft.AspNetCore.Mvc;

namespace Fintech.Backoffice.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // API endpoint for refreshing dashboard data (used by AJAX every 30s)
        [HttpGet]
        public JsonResult GetMetrics()
        {
            var random = new Random();
            var metrics = new
            {
                TotalApplications = 1247 + random.Next(0, 5),
                InProgress = 23 + random.Next(0, 3),
                Rejected = 156,
                Completed = 1042,
                Cancelled = 26,
                TodayApproved = 142500.00m,
                MonthlyApproved = 3650000.00m,
                RejectionPercentage = 12.5m,
                LastUpdated = DateTime.Now.ToString("HH:mm:ss")
            };
            return Json(metrics);
        }

        [HttpGet]
        public JsonResult GetChartData()
        {
            var data = new
            {
                // Daily applications trend (last 7 days)
                DailyApplications = new
                {
                    Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                    Values = new[] { 45, 52, 38, 65, 70, 28, 35 }
                },
                // Status distribution
                StatusDistribution = new
                {
                    Labels = new[] { "Completed", "In Progress", "Rejected", "Cancelled" },
                    Values = new[] { 1042, 23, 156, 26 }
                },
                // Monthly approved amounts
                MonthlyAmounts = new
                {
                    Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" },
                    Values = new[] { 2800000, 3200000, 2950000, 3450000, 3650000, 0 }
                },
                // Partner-wise applications
                PartnerStats = new
                {
                    Labels = new[] { "Emirates Bank", "First National", "FinTech Solutions", "Gulf Credit", "Digital Loans" },
                    Values = new[] { 320, 285, 198, 234, 210 }
                }
            };
            return Json(data);
        }
    }
}
