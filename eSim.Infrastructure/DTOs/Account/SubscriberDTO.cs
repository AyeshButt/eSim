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
    public class SubscriberRequestDTO
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
        public string Password { get; set; }

    }

    public class EmailExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success; // Let [Required] handle null checks

            var email = value.ToString();
            var subscriberService = validationContext.GetRequiredService<ISubscriberService>(); // Your service that checks emails

            if (subscriberService.EmailExists(email).GetAwaiter().GetResult()) // Sync call for validation
            {
                return new ValidationResult("Email already exists.");
            }

            return ValidationResult.Success;
        }
    }

}
