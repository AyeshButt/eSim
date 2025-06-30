using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Inventory;

namespace eSim.Infrastructure.Interfaces.Selfcare.Esim
{
    public interface IEsimService
    {
        public Task<Result<IEnumerable<EsimDTO>>> GetEsimListAsync();
        public Task<Result<GetEsimDetailsResponse>> GetEsimDetailsAsync(string iccid);
        public Task<Result<GetEsimHistoryResponse>> GetEsimHistoryAsync(string iccid);
        public Task<Result<List<SubscriberInventoryResponse>>> GetSubscriberInventoryAsync();
        public Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToExistingEsimAsync(ApplyBundleToExistingEsimRequest input);
        public Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToEsimAsync(ApplyBundleToEsimRequest input);
    }
}
