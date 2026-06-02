using Microsoft.AspNetCore.Mvc;

namespace Fintech.Backoffice.Web.Controllers
{
    public class TagsController : Controller
    {
        // Urgent: In-Progress applications older than 3 days
        public IActionResult Urgent()
        {
            var allApps = GetAllApplications();
            var urgent = allApps
                .Where(a => a.Status == "In-Progress" && (DateTime.Now - a.ApplicationDate).TotalDays >= 2)
                .ToList();

            ViewData["TagName"] = "Urgent";
            ViewData["TagDescription"] = "In-progress applications requiring immediate attention (open more than 2 days)";
            ViewData["TagColor"] = "danger";
            ViewData["TagIcon"] = "fa-exclamation-triangle";
            return View("TagView", urgent);
        }

        // Reviewed: Completed or Rejected applications
        public IActionResult Reviewed()
        {
            var allApps = GetAllApplications();
            var reviewed = allApps
                .Where(a => a.Status == "Completed" || a.Status == "Rejected")
                .OrderByDescending(a => a.ApplicationDate)
                .ToList();

            ViewData["TagName"] = "Reviewed";
            ViewData["TagDescription"] = "Applications that have been processed (completed or rejected)";
            ViewData["TagColor"] = "success";
            ViewData["TagIcon"] = "fa-check-double";
            return View("TagView", reviewed);
        }

        private List<ApplicationViewModel> GetAllApplications()
        {
            return new List<ApplicationViewModel>
            {
                new() { ProcessNumber = "APP-2024-005001", ApplicationId = "APP5001", ApprovedAmount = 75000, Status = "Completed", ApplicationDate = DateTime.Now, CustomerName = "Sara Al-Falahi", MobileNumber = "+971510123456", PartnerName = "Global Finance Hub" },
                new() { ProcessNumber = "APP-2024-005002", ApplicationId = "APP5002", ApprovedAmount = 42500, Status = "Completed", ApplicationDate = DateTime.Now, CustomerName = "Mohammed Al-Mazrouei", MobileNumber = "+971503456789", PartnerName = "Emirates Bank" },
                new() { ProcessNumber = "APP-2024-005003", ApplicationId = "APP5003", ApprovedAmount = 25000, Status = "Completed", ApplicationDate = DateTime.Now, CustomerName = "Ahmed Hassan", MobileNumber = "+971501234567", PartnerName = "FinTech Solutions" },
                new() { ProcessNumber = "APP-2024-001001", ApplicationId = "APP1001", ApprovedAmount = 50000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-25), CustomerName = "Ahmed Hassan", MobileNumber = "+971501234567", PartnerName = "Emirates Bank" },
                new() { ProcessNumber = "APP-2024-001002", ApplicationId = "APP1002", ApprovedAmount = 75000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-24), CustomerName = "Fatima Al-Mansoori", MobileNumber = "+971502345678", PartnerName = "First National Bank" },
                new() { ProcessNumber = "APP-2024-001003", ApplicationId = "APP1003", ApprovedAmount = 45000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-22), CustomerName = "Mohammed Al-Mazrouei", MobileNumber = "+971503456789", PartnerName = "FinTech Solutions" },
                new() { ProcessNumber = "APP-2024-001004", ApplicationId = "APP1004", ApprovedAmount = 120000, Status = "Completed", ApplicationDate = DateTime.Now.AddDays(-20), CustomerName = "Layla Al-Naqbi", MobileNumber = "+971504567890", PartnerName = "Gulf Credit Corp" },
                new() { ProcessNumber = "APP-2024-002001", ApplicationId = "APP2001", ApprovedAmount = 0, Status = "Rejected", ApplicationDate = DateTime.Now.AddDays(-19), CustomerName = "Ali Al-Marri", MobileNumber = "+971505678901", PartnerName = "Digital Loans" },
                new() { ProcessNumber = "APP-2024-003001", ApplicationId = "APP3001", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-5), CustomerName = "Noor Al-Hosani", MobileNumber = "+971506789012", PartnerName = "Smart Finance" },
                new() { ProcessNumber = "APP-2024-003002", ApplicationId = "APP3002", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-3), CustomerName = "Salim Al-Kaabi", MobileNumber = "+971507890123", PartnerName = "Express Credit" },
                new() { ProcessNumber = "APP-2024-003003", ApplicationId = "APP3003", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-2), CustomerName = "Layla Al-Naqbi", MobileNumber = "+971504567890", PartnerName = "First National Bank" },
                new() { ProcessNumber = "APP-2024-003004", ApplicationId = "APP3004", ApprovedAmount = 0, Status = "In-Progress", ApplicationDate = DateTime.Now.AddDays(-1), CustomerName = "Hana Al-Khayeli", MobileNumber = "+971508901234", PartnerName = "Digital Loans" },
                new() { ProcessNumber = "APP-2024-002002", ApplicationId = "APP2002", ApprovedAmount = 0, Status = "Rejected", ApplicationDate = DateTime.Now.AddDays(-16), CustomerName = "Mohammed Al-Mazrouei", MobileNumber = "+971503456789", PartnerName = "Gulf Credit Corp" },
                new() { ProcessNumber = "APP-2024-002003", ApplicationId = "APP2003", ApprovedAmount = 0, Status = "Rejected", ApplicationDate = DateTime.Now.AddDays(-13), CustomerName = "Ali Al-Marri", MobileNumber = "+971505678901", PartnerName = "Smart Finance" }
            };
        }
    }
}
