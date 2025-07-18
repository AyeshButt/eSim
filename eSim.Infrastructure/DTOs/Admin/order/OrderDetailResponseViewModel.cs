using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Middleware.Order;

namespace eSim.Infrastructure.DTOs.Admin.order
{
    public class OrderDetailResponseViewModel
    {
        public List<OrderBySubIdDetail> Order { get; set; } = new();

        public double? Total { get; set;     }
        public string OrderReferenceId { get; set;     }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StatusMessage { get; set; } = null!;
        public string OrderReference { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public bool? Assigned { get; set; }
        public string SourceIP { get; set; } = null!;
    }

    public class OrderBySubIdDetail
    {
        public string Type { get; set; } = null!;
        public string Item { get; set; } = null!;
        public int? Quantity { get; set; }
        public double? SubTotal { get; set; }
        public double? PricePerUnit { get; set; }
        public bool? AllowReassign { get; set; }

    }
}
