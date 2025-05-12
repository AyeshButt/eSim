using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is requird")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requird")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is requird"), Compare("Password", ErrorMessage = "Password and Confirm Password should match") ]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
