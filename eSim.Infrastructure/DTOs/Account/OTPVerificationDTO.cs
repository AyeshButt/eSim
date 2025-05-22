using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs.Account
{
    public class OTPVerificationDTO
    {
        public string Id { get; set; }
        public string? UserId { get; set; }
        [Required(ErrorMessage ="OTP is required")]
        public string OTP { get; set; }
        public DateTime SentTime { get; set; }
        public string Type { get; set; }
        public bool IsValid { get; set; }
        public string? Email { get; set; }

    }
}
