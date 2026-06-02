using Microsoft.AspNetCore.Mvc;

namespace Fintech.Backoffice.Web.Controllers
{
    public class PartnersController : Controller
    {
        public IActionResult Index()
        {
            var partners = new List<PartnerViewModel>
            {
                new() { PartnerName = "Emirates Bank Corporation",     PartnerCode = "EMB001", TotalApps = 320, Completed = 280, Rejected = 22, InProgress = 12, Cancelled = 6,  TotalApproved = 14_500_000m, ApprovalRate = 87.5m,  ContactEmail = "loans@emiratesbank.ae" },
                new() { PartnerName = "First National Bank",           PartnerCode = "FNB001", TotalApps = 285, Completed = 245, Rejected = 25, InProgress = 10, Cancelled = 5,  TotalApproved = 12_750_000m, ApprovalRate = 85.9m,  ContactEmail = "api@fnbank.ae" },
                new() { PartnerName = "FinTech Solutions UAE",         PartnerCode = "FTS001", TotalApps = 198, Completed = 165, Rejected = 20, InProgress = 8,  Cancelled = 5,  TotalApproved = 8_900_000m,  ApprovalRate = 83.3m,  ContactEmail = "partnerships@fintech.ae" },
                new() { PartnerName = "Gulf Credit Corporation",       PartnerCode = "GCC001", TotalApps = 234, Completed = 198, Rejected = 24, InProgress = 8,  Cancelled = 4,  TotalApproved = 10_450_000m, ApprovalRate = 84.6m,  ContactEmail = "integrate@gulfcredit.ae" },
                new() { PartnerName = "Digital Loans Provider",        PartnerCode = "DLP001", TotalApps = 210, Completed = 170, Rejected = 30, InProgress = 7,  Cancelled = 3,  TotalApproved = 9_200_000m,  ApprovalRate = 81.0m,  ContactEmail = "api@digitalloan.ae" },
                new() { PartnerName = "Smart Finance Group",           PartnerCode = "SFG001", TotalApps = 145, Completed = 115, Rejected = 18, InProgress = 8,  Cancelled = 4,  TotalApproved = 6_300_000m,  ApprovalRate = 79.3m,  ContactEmail = "partners@smartfinance.ae" },
                new() { PartnerName = "Express Credit Services",       PartnerCode = "ECS001", TotalApps = 130, Completed = 102, Rejected = 17, InProgress = 7,  Cancelled = 4,  TotalApproved = 5_400_000m,  ApprovalRate = 78.5m,  ContactEmail = "support@expresscredit.ae" },
                new() { PartnerName = "Premium Financial Partners",    PartnerCode = "PFP001", TotalApps = 95,  Completed = 78,  Rejected = 12, InProgress = 3,  Cancelled = 2,  TotalApproved = 4_750_000m,  ApprovalRate = 82.1m,  ContactEmail = "connect@premiumfinance.ae" },
                new() { PartnerName = "Secure Lending Ltd",            PartnerCode = "SLL001", TotalApps = 88,  Completed = 70,  Rejected = 10, InProgress = 6,  Cancelled = 2,  TotalApproved = 4_100_000m,  ApprovalRate = 79.5m,  ContactEmail = "api@securelending.ae" },
                new() { PartnerName = "Global Finance Hub",            PartnerCode = "GFH001", TotalApps = 75,  Completed = 62,  Rejected = 8,  InProgress = 3,  Cancelled = 2,  TotalApproved = 3_650_000m,  ApprovalRate = 82.7m,  ContactEmail = "integration@globalfinance.ae" }
            };
            return View(partners);
        }
    }

    public class PartnerViewModel
    {
        public string PartnerName { get; set; } = "";
        public string PartnerCode { get; set; } = "";
        public string ContactEmail { get; set; } = "";
        public int TotalApps { get; set; }
        public int Completed { get; set; }
        public int Rejected { get; set; }
        public int InProgress { get; set; }
        public int Cancelled { get; set; }
        public decimal TotalApproved { get; set; }
        public decimal ApprovalRate { get; set; }
    }
}
