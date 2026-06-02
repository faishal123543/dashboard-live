using System.Net;
using System.Net.Mail;

namespace Fintech.Backoffice.Web.Services
{
    public interface IEmailService
    {
        Task<bool> SendOtpAsync(string toEmail, string otp);
        bool IsConfigured();
    }

    /// <summary>
    /// Sends emails via SMTP using settings from appsettings.json:Smtp
    /// If SMTP is not configured, falls back to console logging (development mode).
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public bool IsConfigured()
        {
            var host = _config["Smtp:Host"];
            var user = _config["Smtp:User"];
            return !string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(user);
        }

        public async Task<bool> SendOtpAsync(string toEmail, string otp)
        {
            var subject = "FinTech Dashboard - Your Login OTP";
            var body = BuildOtpEmailBody(otp);

            if (!IsConfigured())
            {
                // DEV MODE: SMTP not configured - log to console
                _logger.LogWarning("============================================");
                _logger.LogWarning("SMTP NOT CONFIGURED - DEV MODE");
                _logger.LogWarning("Email To:  {Email}", toEmail);
                _logger.LogWarning("OTP Code:  {Otp}", otp);
                _logger.LogWarning("Expires:   5 minutes");
                _logger.LogWarning("============================================");
                _logger.LogWarning("To send real emails, configure Smtp section in appsettings.json");
                return true; // Pretend success in dev mode
            }

            try
            {
                using var smtp = new SmtpClient(_config["Smtp:Host"])
                {
                    Port = int.Parse(_config["Smtp:Port"] ?? "587"),
                    Credentials = new NetworkCredential(_config["Smtp:User"], _config["Smtp:Password"]),
                    EnableSsl = bool.Parse(_config["Smtp:EnableSsl"] ?? "true")
                };

                var fromEmail = _config["Smtp:FromEmail"] ?? _config["Smtp:User"]!;
                var fromName = _config["Smtp:FromName"] ?? "FinTech Dashboard";

                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(toEmail);

                await smtp.SendMailAsync(message);
                _logger.LogInformation("OTP sent successfully to {Email}", toEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send OTP email to {Email}", toEmail);
                return false;
            }
        }

        private static string BuildOtpEmailBody(string otp)
        {
            return $@"
<!DOCTYPE html>
<html>
<head><meta charset='utf-8'></head>
<body style='font-family: Segoe UI, Arial, sans-serif; background:#0f1419; color:#fff; padding:20px;'>
    <div style='max-width:560px; margin:0 auto; background:#1e2533; border-radius:12px; padding:30px; border:1px solid #2a3550;'>
        <h2 style='color:#00d4ff; margin-top:0;'>FinTech Backoffice Dashboard</h2>
        <p style='color:#a8b2c9;'>Hello,</p>
        <p style='color:#a8b2c9;'>Use the One-Time Password below to log in to your dashboard:</p>
        <div style='background:#0f1419; border:2px dashed #00d4ff; border-radius:8px; padding:20px; text-align:center; margin:24px 0;'>
            <div style='font-size:36px; font-weight:700; color:#00d4ff; letter-spacing:8px;'>{otp}</div>
        </div>
        <p style='color:#a8b2c9; font-size:13px;'>
            This OTP will expire in <strong style='color:#fff;'>5 minutes</strong>.<br/>
            If you didn't request this code, please ignore this email.
        </p>
        <hr style='border:none; border-top:1px solid #2a3550; margin:24px 0;'/>
        <p style='color:#6b7589; font-size:12px; text-align:center; margin:0;'>
            &copy; {DateTime.Now.Year} FinTech Backoffice Dashboard. All rights reserved.
        </p>
    </div>
</body>
</html>";
        }
    }
}
