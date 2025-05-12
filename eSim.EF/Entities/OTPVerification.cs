using System.ComponentModel.DataAnnotations;

namespace eSim.EF.Entities
{
    public class OTPVerification
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string OTP { get; set; }
        public DateTime SentTime { get; set; }
        public string Type { get; set; }
        public bool IsValid { get; set; }

    }
}
