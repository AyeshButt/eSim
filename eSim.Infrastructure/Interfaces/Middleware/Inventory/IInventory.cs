using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
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
    }
}
