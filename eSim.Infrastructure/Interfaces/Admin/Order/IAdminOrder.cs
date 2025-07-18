using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Admin.order;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;

namespace eSim.Infrastructure.Interfaces.Admin.Order
{
    public interface IAdminOrder
    {
        Task<Result<ListOrderResponse>> GetOrdersFromDbAsync(ListOrderRequest request);
        public Task<Result<GetOrderDetailResponse>> GetOrderDetailAsync(string orderReferenceId);

        Task<List<OrderDetailResponseViewModel>> GetOrderBySubscriberId(string Id);
    }
}
