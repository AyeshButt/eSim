using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Subscribers
{
    public class SubscribersResponseViewModel
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
        public string Country { get; set; }

        public string ClientName { get; set; } = null!;
        public string PrimaryEmail { get; set; } = null!;
        public string? Kid { get; set; }
    }
}
