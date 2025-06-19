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

                var selectedBundle = inventory?.Data?.FirstOrDefault(x => x.Item == BundleID);

                if (selectedBundle != null)
                {
                    result.Success = false;
                    result.Message = "No bundle Bundle Found in Inventory";
                    return result;
                }

                Result<GetBundleCatalogueDetailsResponse> bundle = await _bdService.BundleDetail(BundleID);

                if (bundle.Success)
                {
                    result.Data.Quantity = selectedBundle.Quantity;
                    result.Data.CreatedDate = selectedBundle.CreatedDate;
                    result.Data.name = bundle.Data.name;
                    result.Data.roamingEnabled = bundle.Data.roamingEnabled;
                    result.Data.countries = bundle.Data.countries;
                    result.Data.DataAmount = bundle.Data.dataAmount;
                    result.Data.autostart = bundle.Data.autostart;
                    result.Data.description = bundle.Data.description;
                    result.Data.duration = bundle.Data.duration;
                    result.Data.autostart = bundle.Data.autostart;
                    result.Data.Message = bundle.Data.Message;

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
