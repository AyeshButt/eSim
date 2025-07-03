using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Account
{
    public class ChangePasswordDTORequest
    {
       
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._#-])[A-Za-z\d@$!%*?&._#-]{8,}$",
      ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
        public string OldPassword { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._#-])[A-Za-z\d@$!%*?&._#-]{8,}$",
      ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
        public string NewPassword { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._#-])[A-Za-z\d@$!%*?&._#-]{8,}$",
      ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
        public string ConfirmPassword { get; set; }
    }
}
