using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.DTOs.QRDownload;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;

namespace eSim.Implementations.Services.Selfcare.Esim
{
    public class EsimService(IMiddlewareConsumeApi consumeApi) : IEsimService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;

        public async Task<Result<IEnumerable<EsimDTO>>> GetEsimListAsync()
        {
            var url = $"{BusinessManager.MiddlewareBaseURL}/{BusinessManager.EsimList}";

            var request = await _consumeApi.Get<IEnumerable<EsimDTO>>(url);

            return request;
        }   

        public async Task<Result<GetEsimDetailsResponse>> GetEsimDetailsAsync(string iccid)
            {
            var url = $"{BusinessManager.MiddlewareBaseURL}/{BusinessManager.EsimDetails}/{Uri.EscapeDataString(iccid)}?additionalFields={BusinessManager.AppleInstallUrl}";
        
            var request = await _consumeApi.Get<GetEsimDetailsResponse>(url);

            return request;
        }

        public async Task<Result<GetEsimHistoryResponse>> GetEsimHistoryAsync(string iccid)
        {
            var url = $"{BusinessManager.MiddlewareBaseURL}/esims/{Uri.EscapeDataString(iccid)}/history";

            var request = await _consumeApi.Get<GetEsimHistoryResponse>(url);

            return request;
        }

        public async Task<Result<List<SubscriberInventoryResponse>>> GetSubscriberInventoryAsync()
        {
            var url = $"{BusinessManager.MiddlewareBaseURL}/inventory/subscriber";

            var request = await _consumeApi.Get<List<SubscriberInventoryResponse>>(url);

            return request;
        }

        public async Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToExistingEsimAsync(ApplyBundleToExistingEsimRequest input)
        {
            var url = $"{BusinessManager.MiddlewareBaseURL}/esims/apply-bundle-existing-esim";
            var request = await _consumeApi.Post<ApplyBundleToEsimResponse, ApplyBundleToExistingEsimRequest>(url,input);

            return request;     
        }

        public async Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToEsimAsync(ApplyBundleToEsimRequest input)
        {
            var url = $"{BusinessManager.MiddlewareBaseURL}/esims/apply-bundle-new-esim";
            var request = await _consumeApi.Post<ApplyBundleToEsimResponse, ApplyBundleToEsimRequest>(url, input);
                
            return request;
        }

        public async Task<FileDownloadResult> DownloadEsimQRAsync(string iccid)
        {
            var url = $"{BusinessManager.MiddlewareBaseURL}/esims/{iccid}/qr";
           
            var request = await _consumeApi.DownloadQrCodeAsync(url);
                        
            return request;         
        }           
    }
}
