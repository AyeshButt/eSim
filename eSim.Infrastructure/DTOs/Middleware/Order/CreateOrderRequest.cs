using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Order
{
    public class CreateOrderListRequest
    {
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Item { get; set; }
        public List<string> Iccids { get; set; } = new();
        public bool AllowReassign { get; set; }
    }

    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = null!;
        public bool? Assign { get; set; }
        [Required(ErrorMessage = "Order is required")]
        public List<CreateOrderListRequest> Order { get; set; } = new();
        public string? ProfileID { get; set; }
    }

    public class CreateOrderEsimResponse
    {
        public string Iccid { get; set; } = null!;
        public string MatchingId { get; set; } = null!;
        public string SmdpAddress { get; set; } = null!;
    }

    public class CreateOrderListResponse
    {
        public string Type { get; set; } = null!;
        public string Item { get; set; } = null!;
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
        public double PricePerUnit { get; set; }
        public bool AllowReassign { get; set; }
        public List<Esim> Esims { get; set; } = new();
        public List<string> Iccids { get; set; } = new();
    }

    public class CreateOrderResponse
    {
        public List<CreateOrderListResponse> Order { get; set; } = new();
        public double Total { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StatusMessage { get; set; } = null!;
        public string OrderReference { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public bool Assigned { get; set; }
        public string SourceIP { get; set; } = null!;
        public string Message { get; set; } = null!;
    }


}
