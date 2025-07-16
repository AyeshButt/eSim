using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;

namespace eSim.Infrastructure.Interfaces.Middleware.Esim
{
    public interface IEsimService
    {
        Task<Result<IEnumerable<EsimsDTO>>> GetListofEsimsAsync(string loggedUser);
        Task<Result<GetEsimHistoryResponse>> GetEsimHistoryAsync(string iccid,string loggedUser);
        Task<Result<GetEsimInstallDetailReponseDTO>> GetEsimInstallDetailAsync(string reference);
        Task<Result<EsimCompatibilityResponseDTO>> CheckeSIMandBundleCompatibilityAsync(EsimCompatibilityRequestDto request);
        Task<Result<ListBundlesAppliedToEsimResponse>> GetListBundlesAppliedToEsimAsync(string iccid, ListBundlesAppliedToEsimRequest request,string loggedUser);
        Task<Result<byte[]>> DownloadQRAsync(string iccid);
        Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToExistingEsimAsync(ApplyBundleToExistingEsimRequest input, string loggedUser);
        Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToEsimAsync(ApplyBundleToEsimRequest input, string loggedUser);
        Task<Result<GetEsimDetailsResponse>> GetEsimDetailsAsync(string iccid, string? additionalfields,string loggedUser);

    }
}
