using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs.Account
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required"), Compare("Password", ErrorMessage = "Password and Confirm Password should match") ]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
