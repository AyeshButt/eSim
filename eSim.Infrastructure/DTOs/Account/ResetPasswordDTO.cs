using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs.Account
{
    public class ResetPasswordDTO
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]

        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required"), Compare("NewPassword", ErrorMessage = "Password and Confirm Password should match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
