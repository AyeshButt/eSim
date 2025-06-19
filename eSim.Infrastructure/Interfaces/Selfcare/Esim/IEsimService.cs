using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.Selfcare.Esim
{
    public interface IEsimService
    {
        public Task<Result<IEnumerable<EsimDTO>>> GetEsimList();
        public Task<Result<GetEsimHistoryResponseDTO>> GetDetail(string Id);
    }
}
