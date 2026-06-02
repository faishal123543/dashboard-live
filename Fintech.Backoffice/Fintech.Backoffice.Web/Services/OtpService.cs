using System.Collections.Concurrent;

namespace Fintech.Backoffice.Web.Services
{
    public interface IOtpService
    {
        string GenerateOtp(string email);
        bool ValidateOtp(string email, string otp);
        void RemoveOtp(string email);
    }

    /// <summary>
    /// OTP service that generates and validates one-time passwords.
    /// Uses in-memory storage with expiry (5 minutes).
    /// In production, use Redis or database for distributed apps.
    /// </summary>
    public class OtpService : IOtpService
    {
        private static readonly ConcurrentDictionary<string, OtpEntry> _otpStore = new();
        private const int OTP_EXPIRY_MINUTES = 5;

        public string GenerateOtp(string email)
        {
            // Generate 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();

            var entry = new OtpEntry
            {
                Code = otp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(OTP_EXPIRY_MINUTES),
                Attempts = 0
            };

            _otpStore[email.ToLower()] = entry;
            return otp;
        }

        public bool ValidateOtp(string email, string otp)
        {
            var key = email.ToLower();
            if (!_otpStore.TryGetValue(key, out var entry))
                return false;

            // Check expiry
            if (DateTime.UtcNow > entry.ExpiresAt)
            {
                _otpStore.TryRemove(key, out _);
                return false;
            }

            // Limit attempts (max 5)
            entry.Attempts++;
            if (entry.Attempts > 5)
            {
                _otpStore.TryRemove(key, out _);
                return false;
            }

            if (entry.Code == otp)
            {
                _otpStore.TryRemove(key, out _);
                return true;
            }

            return false;
        }

        public void RemoveOtp(string email)
        {
            _otpStore.TryRemove(email.ToLower(), out _);
        }

        private class OtpEntry
        {
            public string Code { get; set; } = "";
            public DateTime ExpiresAt { get; set; }
            public int Attempts { get; set; }
        }
    }
}
