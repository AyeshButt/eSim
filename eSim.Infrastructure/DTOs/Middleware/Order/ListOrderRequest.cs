using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Order
{
    public class ListOrderRequest
    {
        public bool? IncludeIccids { get; set; }
        public int? Page { get; set; }
        public int? Limit { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

    }

    public class Esim
    {
        public string Iccid { get; set; } = null!;
        public string MatchingId { get; set; } = null!;
        public string SmdpAddress { get; set; } = null!;
    }

    public class GetOrderDetailResponse
    {
        public List<OrderInnerDetails> Order { get; set; } = new();
        public double Total { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StatusMessage { get; set; } = null!;
        public string OrderReference { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public double RunningBalance { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SourceIP { get; set; } = null!;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Assigned { get; set; }
        public string Message { get; set; } = null!;
    }

    public class OrderInnerDetails
    {
        public string Type { get; set; } = null!;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Item { get; set; } = null!;
        public int Quantity { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public double SubTotal { get; set; }
        public double PricePerUnit { get; set; }
        public bool AllowReassign { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Esim> Esims { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Iccids { get; set; }
    }

    public class ListOrderResponse
    {
        public List<GetOrderDetailResponse> Orders { get; set; } = new();
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int Rows { get; set; }
    }


}
