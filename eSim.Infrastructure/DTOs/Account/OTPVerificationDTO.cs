namespace eSim.Infrastructure.DTOs.Account
{
    public class OTPVerificationDTO
    {
        public string? UserId { get; set; }
        public string OTP { get; set; }
        public DateTime SentTime { get; set; }
        public string Type { get; set; }
        public bool IsValid { get; set; }
        public string? Email { get; set; }

    }
}
