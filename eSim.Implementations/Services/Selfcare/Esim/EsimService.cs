using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;

namespace eSim.Implementations.Services.Selfcare.Esim
{
    public class EsimService(IMiddlewareConsumeApi consumeApi) : IEsimService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;



        public async Task<Result<GetListofyourEsimsResponseDTO>> GetList()
        {
            var Url = BusinessManager.MdwBaseURL + BusinessManager.EsimList;
            var request = await _consumeApi.Get<GetListofyourEsimsResponseDTO>(Url);
            return request;
        }

        public async Task<Result<GetEsimHistoryResponseDTO>> GetDetail(string Iccid)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.EsimHistory;

        
            var FullURl = $"{url}?iccid={Uri.EscapeDataString(Iccid)}";

            var request = await _consumeApi.Get<GetEsimHistoryResponseDTO>(FullURl);

            return request;
        }
    }
}
