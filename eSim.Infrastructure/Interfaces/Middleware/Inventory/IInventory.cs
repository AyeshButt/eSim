using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.DTOs.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.Middleware.Inventory
{
    public interface IInventory
    {
        public Task<Result<GetBundleInventoryResponse>> GetBundleInventoryAsync();
        public Task<Result<string>> AddBundleInventoryAsync(CreateOrderResponse input, string subscriberId);
        public Task<Result<List<SubscriberInventoryResponse>>> GetSubscriberInventoryResponse(string subscriberId);
        public Task<Result<string>> RefundBundleAsync(RefundBundleDataBaseRequest input, string subscriberId);

    }
}
