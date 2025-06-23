using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.DTOs.Selfcare.Inventory;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.Selfcare.Inventory;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Implementations.Services.Selfcare.Inventory
{
    public class InventoryService(IMiddlewareConsumeApi consumeApi, IBundleService bdService) : IInventoryService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;
        private readonly IBundleService _bdService = bdService;

        

        public async Task<Result<List<SubscriberInventoryResponse>>> GetListAsync()
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.SubscriberInventory;
            var request = await _consumeApi.Get<List<SubscriberInventoryResponse>>(url);
            return request;
        }

        public async Task<Result<SubscriberInventoryResponseViewModel>> DetailAsync(string BundleID)
        {
            Result<SubscriberInventoryResponseViewModel> result = new();


            try
            {
                var inventory = await GetListAsync();

                var selectedBundle = inventory.Data.FirstOrDefault(x => x.Item == BundleID);

                if (selectedBundle == null)
                {
                    result.Success = false;
                    result.Message = "No bundle Bundle Found in Inventory";
                    return result;
                }

                Result<GetBundleCatalogueDetailsResponse> bundle = await _bdService.BundleDetail(BundleID);

                if (bundle.Success && bundle.Data != null)
                {
                    result.Data = new SubscriberInventoryResponseViewModel
                    {
                        Quantity = selectedBundle.Quantity,
                        CreatedDate = selectedBundle.CreatedDate,
                        name = bundle.Data.name,
                        roamingEnabled = bundle.Data.roamingEnabled,
                        countries = bundle.Data.countries,
                        dataAmount = bundle.Data.dataAmount,
                        autostart = bundle.Data.autostart,
                        description = bundle.Data.description,
                        duration = bundle.Data.duration,
                        Message = bundle.Data.Message
                    };

                    result.Success = true;
                    return result;
                }
                result.Success = false;
                result.Message = "something went weong";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message =ex.Message;
                return result;
            }
        }

    }
}
