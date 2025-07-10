using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Admin.Inventory
{
    public class AdminInventoryViewModel
    {
        public string? Client { get; set; }
        public string? Subscriber { get; set; }
        public string? Date { get; set; }
        public List<AdminInventoryDTO> Inventory { get; set; } = new List<AdminInventoryDTO>();
    }
    public class AdminInventoryFilterDTO
    {
        public string? Client { get; set; }
        public string? Subscriber { get; set; }
        public string? Date { get; set; }

    }
    public class AdminInventoryDTO
    {
        public Guid Id { get; set; }
        public Guid SubscriberId { get; set; }
        public Guid ClientId { get; set; }
        public string OrderRefrenceId { get; set; } = null!;
        public string? Type { get; set; }
        public string Item { get; set; } = null!;
        public string? Description { get; set; }
        public int? DataAmount { get; set; }
        public int? Duration { get; set; }
        public string? Country { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Assigned { get; set; }
        public bool? AllowReassign { get; set; }
        public string Client { get; set; } = null!;
        public string Subscriber { get; set; } = null!;
    }
}
