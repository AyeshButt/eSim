using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.DTOs.Selfcare.Inventory;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.Selfcare.Inventory;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;
using static Raven.Client.Linq.LinqPathProvider;

namespace eSim.Implementations.Services.Selfcare.Inventory
{
    public class InventoryService(IMiddlewareConsumeApi consumeApi, IBundleService bdService) : IInventoryService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;
        private readonly IBundleService _bdService = bdService;

        #region Get iNventory List

        public async Task<Result<List<SubscriberInventoryResponse>>> GetListAsync()
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.SubscriberInventory;
            var request = await _consumeApi.Get<List<SubscriberInventoryResponse>>(url);
            return request;
        }
        #endregion

        #region Detail of bundle from inventory

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
                result.Message = ex.Message;
                return result;
            }
        }

        #endregion

        #region Genrate new esim against the bundle form Inventory
        public async Task<Result<ApplyBundleToEsimResponse>> GenrateAsync(string name) 
        {
            Result<ApplyBundleToEsimResponse> model = new();
            try
            {
                var url = BusinessManager.MdwBaseURL + BusinessManager.ApplyNewBundle;

                if (name == null)
                {
                    model.Success = false;
                    model.Message = "Invalid Bundle name";
                    return model;
                }
                ApplyBundleToEsimRequest inputDTO = new ApplyBundleToEsimRequest()
                {
                    Name = name
                };

                var bundle = await VerifyBundle(inputDTO.Name);

                if (!bundle)
                {
                    model.Success = false;
                    model.Message = "Invalid Bundle";
                    return model;
                }

                var applyRequest = await _consumeApi.Post<ApplyBundleToEsimResponse, ApplyBundleToEsimRequest>(url, inputDTO);

                if (applyRequest.Success && applyRequest.Data != null) 
                {
                    return applyRequest;
                }
                else
                {
                    model.Success = false;
                    model.Message = applyRequest.Message;
                    return model;
                }

            }
            catch (Exception ex)
            {
                model.Success = false;
                model.Message = ex.Message;
                return model;
            }
        }
        #endregion

        #region Detail of bundle from inventory
        public async Task<Result<byte[]>> GenrateQR(string input)
        {
            string QrCode = BusinessManager.QRPath(input);

            var url = $"{BusinessManager.MdwBaseURL}{QrCode}";

            var QrCodeRequest = await _consumeApi.Get<byte[]>(url);

            return QrCodeRequest;
        }
        #endregion

        #region Function to verify the bundle if Exists in Actual Inventory
        private async Task<bool> VerifyBundle(string name)
        {
            var inventory = await GetListAsync();

            var selectedBundle = inventory.Data.FirstOrDefault(x => x.Item == name);

            if (selectedBundle == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

       



    }
}
