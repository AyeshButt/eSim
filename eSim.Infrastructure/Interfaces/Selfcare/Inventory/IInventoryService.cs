using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.DTOs.Selfcare.Inventory;

namespace eSim.Infrastructure.Interfaces.Selfcare.Inventory
{
    public interface IInventoryService
    {
        public Task<Result<List<SubscriberInventoryResponse>>> GetListAsync();
        public Task<Result<SubscriberInventoryResponseViewModel>> DetailAsync(string BundleID);
    }
}
