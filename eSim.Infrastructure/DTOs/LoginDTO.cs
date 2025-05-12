using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class ConfigDTO
    {
        public string? ConnectionString { get; set; }
        public string? KeyId { get; set; }
    }
}
