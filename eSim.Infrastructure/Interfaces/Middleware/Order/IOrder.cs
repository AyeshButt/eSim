using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.Middleware.Order
{
    public interface IOrder
    {
        public Task<Result<CreateOrderResponse>> CreateOrderAsync(CreateOrderDTO input,string loggedUser);
        public Task<Result<ListOrderResponse>> ListOrderAsync(ListOrderRequest input);
        public Task<Result<GetOrderDetailResponse>> GetOrderDetailAsync(string orderReferenceId);
    }
}
