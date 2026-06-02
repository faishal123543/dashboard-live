using Fintech.Backoffice.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fintech.Backoffice.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IOtpService otpService, IEmailService emailService, ILogger<AccountController> logger)
        {
            _otpService = otpService;
            _emailService = emailService;
            _logger = logger;
        }

        // GET /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // If already logged in, go to dashboard
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
                return RedirectToAction("Index", "Dashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST /Account/Login
        // Generates an OTP and emails it. Stores the email in session pending verification.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            var email = model.Email.Trim().ToLower();

            // Generate OTP
            var otp = _otpService.GenerateOtp(email);

            // Send via email
            var sent = await _emailService.SendOtpAsync(email, otp);

            if (!sent)
            {
                ModelState.AddModelError("", "Failed to send OTP. Please try again.");
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            // Store pending email in session
            HttpContext.Session.SetString("PendingEmail", email);

            // In dev mode (no SMTP), pass the OTP to the verify page so it can be shown
            if (!_emailService.IsConfigured())
            {
                TempData["DevModeOtp"] = otp;
            }

            TempData["SuccessMessage"] = $"OTP sent to {email}. Check your email.";
            return RedirectToAction("VerifyOtp", new { returnUrl });
        }

        // GET /Account/VerifyOtp
        [HttpGet]
        public IActionResult VerifyOtp(string? returnUrl = null)
        {
            var email = HttpContext.Session.GetString("PendingEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            ViewData["Email"] = email;
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["DevModeOtp"] = TempData["DevModeOtp"];
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            return View();
        }

        // POST /Account/VerifyOtp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyOtp(VerifyOtpModel model, string? returnUrl = null)
        {
            var email = HttpContext.Session.GetString("PendingEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
            {
                ViewData["Email"] = email;
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            if (!_otpService.ValidateOtp(email, model.Otp))
            {
                ModelState.AddModelError("", "Invalid or expired OTP. Please try again.");
                ViewData["Email"] = email;
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            // Success: log the user in
            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserName", GetDisplayNameFromEmail(email));
            HttpContext.Session.SetString("LoginTime", DateTime.UtcNow.ToString("o"));
            HttpContext.Session.Remove("PendingEmail");

            _logger.LogInformation("User {Email} logged in successfully", email);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        // POST /Account/ResendOtp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendOtp()
        {
            var email = HttpContext.Session.GetString("PendingEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            var otp = _otpService.GenerateOtp(email);
            await _emailService.SendOtpAsync(email, otp);

            if (!_emailService.IsConfigured())
                TempData["DevModeOtp"] = otp;

            TempData["SuccessMessage"] = $"A new OTP has been sent to {email}";
            return RedirectToAction("VerifyOtp");
        }

        // GET/POST /Account/Logout
        public IActionResult Logout()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            HttpContext.Session.Clear();
            _logger.LogInformation("User {Email} logged out", email ?? "(unknown)");
            return RedirectToAction("Login");
        }

        private static string GetDisplayNameFromEmail(string email)
        {
            // e.g. "mike.taylor@example.com" -> "Mike Taylor"
            var local = email.Split('@')[0];
            var parts = local.Split('.', '_', '-');
            return string.Join(" ", parts.Select(p =>
                string.IsNullOrEmpty(p) ? p : char.ToUpper(p[0]) + p[1..]));
        }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = "";
    }

    public class VerifyOtpModel
    {
        [Required(ErrorMessage = "OTP is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be 6 digits")]
        public string Otp { get; set; } = "";
    }
}
