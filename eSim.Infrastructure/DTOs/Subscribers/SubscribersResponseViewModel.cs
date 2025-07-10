using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Admin.Inventory;

namespace eSim.Infrastructure.DTOs.Subscribers
{
   public class SubscribersResponseViewModel
    {
        public string? Client { get; set; }
        public string? Subscriber { get; set; }
        public string? DateRange {  get; set; }
        public List<SubscribersResponseDTO> SubscribersResponse { get; set; } = new List<SubscribersResponseDTO>();

    }

    public class SubscribersResponseDTO
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
