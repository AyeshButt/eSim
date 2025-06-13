using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Azure;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware.Esim;
using Microsoft.AspNetCore.Http;

namespace eSim.Implementations.Services.Esim
{
    public class EsimService : IEsimService
    {
        private readonly IConsumeApi _consumeApi;
        public EsimService(IConsumeApi consumeApi)
        {
            _consumeApi = consumeApi;
        }
        #region  CheckeSIMandBundleCompatibility
        public async Task<Result<EsimCompatibilityResponseDTO>> CheckeSIMandBundleCompatibilityAsync(EsimCompatibilityRequestDto request)
        {
            var result = new Result<EsimCompatibilityResponseDTO>();
            string url = $" {BusinessManager.BaseURL}/esims/{request.Iccid}/compatible/{request.Bundle}";


            try
            {
                var response = await _consumeApi.GetApi<EsimCompatibilityResponseDTO>(url);

                if (response == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimNotCompatible;
                    return result;
                }
                if (response.Message is not null) 
                
                {
                    result.Success = false;
                    result.Message = response.Message;
                    return result;
                }
                result.Success = response.Compatible;
                result.Data = response;
                result.Message = response.Compatible ? BusinessManager.EsimCompatible : BusinessManager.EsimNotCompatible;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        #endregion

        public async Task<Result<byte[]>> DownloadQRAsync(string iccid)
        {
            var result = new Result<byte[]>();
            
            string url = $"{BusinessManager.BaseURL}/esims/{iccid}/qr";
            
            try
            {
                result = await _consumeApi.GetApii<byte[]>(url);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        #region EsimBundleInventory
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
        
        #region GetEsimInstallDetail
        public async Task<Result<GetEsimInstallDetailReponseDTO>> GetEsimInstallDetailAsync(string reference)


        {
            var result = new Result<GetEsimInstallDetailReponseDTO>();
            if (string.IsNullOrWhiteSpace(reference))
            {
                result.Success = false;
                result.Message = BusinessManager.Missingreference; 
                return result;
            }
            string url = $"{BusinessManager.BaseURL}/esims/assignments?reference={reference}";

            try
            {
                var response = await _consumeApi.GetApi<GetEsimInstallDetailReponseDTO>(url);

                if (response == null )
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimInstallDetailNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.EsimInstallDetail;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion
        
        #region  GetListBundlesappliedtoeSIM
        public async Task<Result<ListBundlesAppliedToESIMResponseDTO>> GetListBundlesappliedtoeSIMAsync(ListBundlesAppliedToESIMRequestDTO request)
        {
            var result = new Result<ListBundlesAppliedToESIMResponseDTO>();

            try
            {
                string url = $"{BusinessManager.BaseURL}/esims/{request.iccid}/bundles";

                if (request.includeUsed.HasValue)
                    url += $"?includeUsed={request.includeUsed.Value.ToString().ToLower()}" +
                           (!string.IsNullOrEmpty(request.limit?.ToString()) ? $"&limit={request.limit}" : "");
                else if (request.limit.HasValue)
                    url += $"?limit={request.limit}";

                var response = await _consumeApi.GetApi<ListBundlesAppliedToESIMResponseDTO>(url);

                if (response == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.BundleNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.ListofEsimBundleFetched;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error retrieving bundles: {ex.Message}.";
            }

            return result;
        }


        #endregion

        #region EsimList
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

