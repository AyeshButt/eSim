using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace eSim.Infrastructure.DTOs.Account
{
    public class SubscriberDTORequest
    {
        [Required]
        public string MerchantId { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } 
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(500)]
        [EmailAddress]
        [EmailExists]

        
        public string Email { get; set; } 

        [Required]
        [MaxLength(75)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._-])[A-Za-z\d@$!%*?&._-]{8,}$",
      ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
        public string Password { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        public string Country { get; set; }


    }

    public class EmailExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            var email = value.ToString();
            var subscriberService = validationContext.GetRequiredService<ISubscriberService>();

            var result = subscriberService.SubscriberEmailExists(email).GetAwaiter().GetResult(); 

            if (result.Message == "Email already exists.") 
            {
                return new ValidationResult("Email already exists.");
            }

            return ValidationResult.Success;
        }
    }


}
