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
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;

namespace eSim.Implementations.Services.Selfcare.Esim
{
    public class EsimService(IMiddlewareConsumeApi consumeApi) : IEsimService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;

        public async Task<Result<IEnumerable<EsimDTO>>> GetEsimListAsync()
        {
            var url = $"{BusinessManager.MdwBaseURL}/{BusinessManager.EsimList}";

            var request = await _consumeApi.Get<IEnumerable<EsimDTO>>(url);

            return request;
        }

        public async Task<Result<GetEsimDetailsResponse>> GetEsimDetailsAsync(string iccid)
        {
            var url = $"{BusinessManager.MdwBaseURL}/{BusinessManager.EsimDetails}/{Uri.EscapeDataString(iccid)}?additionalFields={BusinessManager.AppleInstallUrl}";
        
            var request = await _consumeApi.Get<GetEsimDetailsResponse>(url);

            return request;
        }

        public async Task<Result<GetEsimHistoryResponse>> GetEsimHistoryAsync(string iccid)
        {
            var url = $"{BusinessManager.MdwBaseURL}/inventory/{Uri.EscapeDataString(iccid)}/history";

            var request = await _consumeApi.Get<GetEsimHistoryResponse>(url);

            return request;
        }

        public async Task<Result<List<SubscriberInventoryResponse>>> GetSubscriberInventoryAsync()
        {
            var url = $"{BusinessManager.MdwBaseURL}/inventory/subscriber";

            var request = await _consumeApi.Get<List<SubscriberInventoryResponse>>(url);

            return request;
        }

        public async Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToExistingEsimAsync(ApplyBundleToExistingEsimRequest input)
        {
            var url = $"{BusinessManager.MdwBaseURL}/esims/apply-bundle-existing-esim";

            var request = await _consumeApi.Post<ApplyBundleToEsimResponse, ApplyBundleToExistingEsimRequest>(url,input);

            return request;
        }
    }
}
