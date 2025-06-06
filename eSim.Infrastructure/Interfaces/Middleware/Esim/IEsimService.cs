using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.Middleware.Esim
{
    public interface IEsimService
    {
        Task<Result<GetListofyourEsimsResponseDTO>> GetListofEsimsAsync();
        Task<Result<GetEsimHistoryResponseDTO>> GetEsimHistoryAsync(string iccid);
        Task<Result<GetBundleInventoryDTO>> GetEsimBundleInventoryAsync();

    }
}
