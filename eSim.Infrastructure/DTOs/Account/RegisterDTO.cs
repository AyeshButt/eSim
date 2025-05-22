using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs.Account
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required"), Compare("Password", ErrorMessage = "Password and confirm password should match") ]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
