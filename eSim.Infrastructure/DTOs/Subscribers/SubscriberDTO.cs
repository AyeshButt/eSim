using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Subscribers
{
    public class SubscriberDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Hash { get; set; } = null!;
        public bool Active { get; set; }
        public Guid ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string? ProfileImage { get; set; }
        public string Country { get; set; }
        public bool IsEmailVerifired { get; set; } = false;
        public bool TermsAndConditions { get; set; }
    }
}
