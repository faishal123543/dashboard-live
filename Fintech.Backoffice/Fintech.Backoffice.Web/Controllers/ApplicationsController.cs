using Microsoft.AspNetCore.Mvc;

namespace Fintech.Backoffice.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        // Main page with optional status filter
        // /Applications              -> All
        // /Applications?status=In-Progress
        // /Applications?status=Completed
        // /Applications?status=Rejected
        // /Applications?status=Cancelled
        public IActionResult Index(string? status = null)
        {
            var allApplications = GetSampleApplications();

            // Apply status filter if provided
            var filtered = string.IsNullOrEmpty(status)
                ? allApplications
                : allApplications.Where(a =>
                    a.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();

            // Set view title based on status
            ViewData["StatusFilter"] = status;
            ViewData["PageTitle"] = status switch
            {
                "In-Progress" => "In-Progress Applications",
                "Completed" => "Completed Applications",
                "Rejected" => "Rejected Applications",
                "Cancelled" => "Cancelled Applications",
                _ => "All Applications"
            };

            return View(filtered);
        }

        // Convenience action routes (optional - the query string approach above is primary)
        public IActionResult InProgress() => Index("In-Progress");
        public IActionResult Completed() => Index("Completed");
        public IActionResult Rejected() => Index("Rejected");
        public IActionResult Cancelled() => Index("Cancelled");

        private List<ApplicationViewModel> GetSampleApplications()
        {
            return new List<ApplicationViewModel>
            {
                // Today's completed applications (for Daily Approved Amount)
                new() { ProcessNumber = "APP-2024-005001", ApplicationId = "APP5001", ApprovedAmount = 75000, Status = "Completed", ApplicationDate = DateTime.Now, CustomerName = "Sara Al-Falahi", MobileNumber = "+971510123456", PartnerName = "Global Finance Hub" },
                new() { ProcessNumber = "APP-2024-005002", ApplicationId = "APP5002", ApprovedAmount = 42500, Status = "Completed", ApplicationDate = DateTime.Now, CustomerName = "Mohammed Al-Mazrouei", MobileNumber = "+971503456789", PartnerName = "Emirates Bank" },
                new() { ProcessNumber = "APP-2024-005003", ApplicationId = "APP5003", ApprovedAmount = 25000, Status = "Completed", ApplicationDate = DateTime.Now, CustomerName = "Ahmed Hassan", MobileNumber = "+971501234567", PartnerName = "FinTech Solutions" },

                // Older applications
                new() { ProcessNumber = "APP-2024-001001", ApplicationId = "APP1001", ApprovedAmount = 50000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-25), CustomerName = "Ahmed Hassan", MobileNumber = "+971501234567", PartnerName = "Emirates Bank" },
                new() { ProcessNumber = "APP-2024-001002", ApplicationId = "APP1002", ApprovedAmount = 75000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-24), CustomerName = "Fatima Al-Mansoori", MobileNumber = "+971502345678", PartnerName = "First National Bank" },
                new() { ProcessNumber = "APP-2024-001003", ApplicationId = "APP1003", ApprovedAmount = 45000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-22), CustomerName = "Mohammed Al-Mazrouei", MobileNumber = "+971503456789", PartnerName = "FinTech Solutions" },
                new() { ProcessNumber = "APP-2024-001004", ApplicationId = "APP1004", ApprovedAmount = 120000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-20), CustomerName = "Layla Al-Naqbi", MobileNumber = "+971504567890", PartnerName = "Gulf Credit Corp" },
                new() { ProcessNumber = "APP-2024-002001", ApplicationId = "APP2001", ApprovedAmount = 0, Status = "Rejected", ApplicationDate = DateTime.Now.AddDays(-19), CustomerName = "Ali Al-Marri", MobileNumber = "+971505678901", PartnerName = "Digital Loans" },
                new() { ProcessNumber = "APP-2024-003001", ApplicationId = "APP3001", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-5), CustomerName = "Noor Al-Hosani", MobileNumber = "+971506789012", PartnerName = "Smart Finance" },
                new() { ProcessNumber = "APP-2024-003002", ApplicationId = "APP3002", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-3), CustomerName = "Salim Al-Kaabi", MobileNumber = "+971507890123", PartnerName = "Express Credit" },
                new() { ProcessNumber = "APP-2024-004001", ApplicationId = "APP4001", ApprovedAmount = 0, Status = "Cancelled", ApplicationDate = DateTime.Now.AddDays(-21), CustomerName = "Hana Al-Khayeli", MobileNumber = "+971508901234", PartnerName = "Premium Financial" },
                new() { ProcessNumber = "APP-2024-001005", ApplicationId = "APP1005", ApprovedAmount = 65000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-18), CustomerName = "Rashid Al-Suwaidi", MobileNumber = "+971509012345", PartnerName = "Secure Lending" },
                new() { ProcessNumber = "APP-2024-001006", ApplicationId = "APP1006", ApprovedAmount = 95000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-15), CustomerName = "Sara Al-Falahi", MobileNumber = "+971510123456", PartnerName = "Global Finance Hub" },
                new() { ProcessNumber = "APP-2024-001007", ApplicationId = "APP1007", ApprovedAmount = 55000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-12), CustomerName = "Ahmed Hassan", MobileNumber = "+971501234567", PartnerName = "Express Credit" },
                new() { ProcessNumber = "APP-2024-002002", ApplicationId = "APP2002", ApprovedAmount = 0, Status = "Rejected", ApplicationDate = DateTime.Now.AddDays(-16), CustomerName = "Mohammed Al-Mazrouei", MobileNumber = "+971503456789", PartnerName = "Gulf Credit Corp" },
                new() { ProcessNumber = "APP-2024-003003", ApplicationId = "APP3003", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-2), CustomerName = "Layla Al-Naqbi", MobileNumber = "+971504567890", PartnerName = "First National Bank" },
                new() { ProcessNumber = "APP-2024-001008", ApplicationId = "APP1008", ApprovedAmount = 80000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-10), CustomerName = "Noor Al-Hosani", MobileNumber = "+971506789012", PartnerName = "Premium Financial" },
                new() { ProcessNumber = "APP-2024-001009", ApplicationId = "APP1009", ApprovedAmount = 70000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-8), CustomerName = "Salim Al-Kaabi", MobileNumber = "+971507890123", PartnerName = "Secure Lending" },
                new() { ProcessNumber = "APP-2024-002003", ApplicationId = "APP2003", ApprovedAmount = 0, Status = "Rejected", ApplicationDate = DateTime.Now.AddDays(-13), CustomerName = "Ali Al-Marri", MobileNumber = "+971505678901", PartnerName = "Smart Finance" },
                new() { ProcessNumber = "APP-2024-003004", ApplicationId = "APP3004", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-1), CustomerName = "Hana Al-Khayeli", MobileNumber = "+971508901234", PartnerName = "Digital Loans" },
                new() { ProcessNumber = "APP-2024-004002", ApplicationId = "APP4002", ApprovedAmount = 0, Status = "Cancelled", ApplicationDate = DateTime.Now.AddDays(-17), CustomerName = "Rashid Al-Suwaidi", MobileNumber = "+971509012345", PartnerName = "Express Credit" }
            };
        }
    }

    public class ApplicationViewModel
    {
        public string ProcessNumber { get; set; } = "";
        public string ApplicationId { get; set; } = "";
        public decimal ApprovedAmount { get; set; }
        public string Status { get; set; } = "";
        public DateTime ApplicationDate { get; set; }
        public string CustomerName { get; set; } = "";
        public string MobileNumber { get; set; } = "";
        public string PartnerName { get; set; } = "";
    }
}
