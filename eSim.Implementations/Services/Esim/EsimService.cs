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
        public async Task<Result<GetBundleInventoryDTORequest>> GetEsimBundleInventoryAsync()
        {
            var result = new Result<GetBundleInventoryDTORequest>();
            string url = $" {BusinessManager.BaseURL}/inventory";

            try
            {
                var response = await _consumeApi.GetApi<GetBundleInventoryDTORequest>(url);

                if (response == null  || !response.bundles.Any())
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.EsimDataFetched;
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

        public async Task<Result<GetEsimHistoryResponseDTO>> GetEsimHistoryAsync(string iccid)
        {
            var result = new Result<GetEsimHistoryResponseDTO>();
            string url = $"{BusinessManager.BaseURL}/esims/{iccid}/history";

            try
            {
                var response = await _consumeApi.GetApi<GetEsimHistoryResponseDTO>(url);

                if (response == null ||  !response.Actions.Any())
                {
                    result.Success = false;
                    result.Message =BusinessManager.EsimNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.EsimDataFetched;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

        # region EsimList
        public async Task<Result<GetListofyourEsimsResponseDTO>> GetListofEsimsAsync()
        {
            var result = new Result<GetListofyourEsimsResponseDTO>();
            string url = $" {BusinessManager.BaseURL}/esims";

            try
            {
                var response = await _consumeApi.GetApi<GetListofyourEsimsResponseDTO>(url);

                if (response == null ||  !response.Esims.Any())
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.EsimDataFetched;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion
    }
}
