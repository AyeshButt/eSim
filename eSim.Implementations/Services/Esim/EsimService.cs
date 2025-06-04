using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware.Esim;

namespace eSim.Implementations.Services.Esim
{
    public class EsimService : IEsimService
    {
        private readonly IConsumeApi _consumeApi;
        public EsimService(IConsumeApi consumeApi)
        {
            _consumeApi = consumeApi;
        }
        # region EsimBundleInventory
        public async Task<Result<BundleInventoryDTO>> GetEsimBundleInventoryAsync()
        {
            var result = new Result<BundleInventoryDTO>();
            string url = $" {BusinessManager.BaseURL}/inventory";

            try
            {
                var response = await _consumeApi.GetApi<BundleInventoryDTO>(url);

                if (response == null || response.bundles == null || !response.bundles.Any())
                {
                    result.Success = false;
                    result.Message = "No eSIM data found.";
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = "eSIM data fetched successfully.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        
        #endregion
        #region EsimHistory

        public async Task<Result<EsimHistoryResponseDTO>> GetEsimHistoryAsync(string iccid)
        {
            var result = new Result<EsimHistoryResponseDTO>();
            string url = $"https://api.esim-go.com/v2.5/esims/{iccid}/history";

            try
            {
                var response = await _consumeApi.GetApi<EsimHistoryResponseDTO>(url);

                if (response == null || response.Actions == null || !response.Actions.Any())
                {
                    result.Success = false;
                    result.Message = "No eSIM data found.";
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = "eSIM data fetched successfully.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        #endregion

        # region EsimList
        public async Task<Result<EsimResponseDTO>> GetEsimsAsync()
        {
            var result = new Result<EsimResponseDTO>();
            string url = $" {BusinessManager.BaseURL}/esims";

            try
            {
                var response = await _consumeApi.GetApi<EsimResponseDTO>(url);

                if (response == null || response.Esims == null || !response.Esims.Any())
                {
                    result.Success = false;
                    result.Message = "No eSIM data found.";
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = "eSIM data fetched successfully.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        #endregion
    }
}
