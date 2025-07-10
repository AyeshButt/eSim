using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Middleware.Order;

namespace eSim.Infrastructure.DTOs.Admin.order
{
    public class OrderViewModel
    {
        public List<GetOrderDetailResponse> Orders { get; set; } = new();
        public string? OrderReference { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int Rows { get; set; }
        public string? Date { get; set; }
    }
}
