using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Inventory
{
    public class SubscriberInventoryResponse
    {
        public Guid SubscriberId { get; set; }
        public string OrderRefrenceId { get; set; }
        public string? Type { get; set; }
        public string Item { get; set; }
        public string? Description { get; set; }
        public int? DataAmount { get; set; }
        public int? Duration { get; set; }
        public string? Country { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Assigned { get; set; }
        public bool? AllowReassign { get; set; }
    }
}
