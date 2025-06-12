using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Implementations.Services.Middleware.Inventory
{
    public class InventoryService : IInventory
    {
        private readonly ApplicationDbContext _db;
        private readonly IConsumeApi _consume;

        public InventoryService(IConsumeApi consume, ApplicationDbContext db)
        {
            _consume = consume;
            _db = db;
        }

        public async Task<Result<GetBundleInventoryResponse>> GetBundleInventoryAsync()
        {
            var result = new Result<GetBundleInventoryResponse>();

            string url = $"{BusinessManager.BaseURL}/inventory";

            try
            {
                var response = await _consume.GetApi<GetBundleInventoryResponse>(url);

                if(response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;

                    return result;
                }

                result.Data = response;
            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

            return result;
        }
    }
}
