using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;

namespace eSim.Infrastructure.Interfaces.Middleware.Esim
{
    public interface IEsimService
    {
        Task<Result<GetListofyourEsimsResponseDTO>> GetListofEsimsAsync();
        Task<Result<GetEsimHistoryResponseDTO>> GetEsimHistoryAsync(string iccid);
        Task<Result<GetBundleInventoryDTORequest>> GetEsimBundleInventoryAsync();
        Task<Result<GetEsimInstallDetailReponseDTO>> GetEsimInstallDetailAsync(string reference);
        Task<Result<EsimCompatibilityResponseDTO>> CheckeSIMandBundleCompatibilityAsync(EsimCompatibilityRequestDto request);
        Task<Result<ListBundlesAppliedToESIMResponseDTO>> GetListBundlesappliedtoeSIMAsync(ListBundlesAppliedToESIMRequestDTO request);


    }
}
