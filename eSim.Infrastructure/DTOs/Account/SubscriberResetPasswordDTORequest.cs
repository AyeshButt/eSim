using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Account
{
    public class SubscriberResetPasswordDTORequest
    {
        //[Required(ErrorMessage = "Password is required")]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }



        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._-])[A-Za-z\d@$!%*?&._-]{8,}$",
      ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required"), Compare("NewPassword", ErrorMessage = "Password and Confirm Password should match")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._-])[A-Za-z\d@$!%*?&._-]{8,}$",
      ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
        public string ConfirmPassword { get; set; }
    }
}
