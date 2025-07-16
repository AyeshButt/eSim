using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Admin.Esim;
using eSim.Infrastructure.DTOs.Admin.Ticket;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;

namespace eSim.Infrastructure.Interfaces.Admin.Esim
{
    public interface IEsims
    {
        Task<List<EsimsDTO>> GetAllAsync(string id);
        Task<IQueryable<EsimsList>> GetEsimListForAllSubscribersAsync();
        Task<Result<GetEsimDetailsResponse>> GetEsimDetailsAsync(string iccid);
        Task<Result<ListBundlesAppliedToEsimResponse>> GetListBundlesAppliedToEsimAsync(string iccid);
        Task<Result<GetEsimHistoryResponse>> GetEsimHistoryAsync(string iccid);

        Task<List<TicketResponseViewModel>> GetAllTicketAsync(string id);
    }
}
