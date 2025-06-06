using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Infrastructure.DTOs.Selfcare.Subscriber
{
    public class SubscriberViewModel
    {
        [Required]
        public string MerchantId { get; set; } = "Inovedia";

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(500)]
        [EmailAddress]
        [Remote(action: "CheckEmail", controller: "Authentication", ErrorMessage = "Email is already exist")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(75)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._-])[A-Za-z\d@$!%*?&._-]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "country is required")]
        public string Country { get; set; }
    }
}
