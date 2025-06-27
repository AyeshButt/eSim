using eSim.Infrastructure.DTOs.Middleware.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Order
{
    #region Create Order Response Payload
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

    #endregion

    #region Create Order Request Payload
    public class CreateOrderDTO
    {
        public required string Type { get; set; } = null!;
        public bool? Assign { get; set; } = false;
        public required List<CreateOrderDetailDTO> Order { get; set; } = new();
    }
    public class CreateOrderRequest
    {
        [Required]
        public List<CreateOrderDetailDTO> Order { get; set; } = new List<CreateOrderDetailDTO>();
    }
    public class CreateOrderDetailDTO
    {
        [Required]
        public string Item { get; set; } = null!;
        public int Quantity { get; set; } = 1;
        [Required]
        public string Type { get; set; } = null!;
    }

    #endregion
}
