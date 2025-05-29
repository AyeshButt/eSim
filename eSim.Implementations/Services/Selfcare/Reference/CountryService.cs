using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Refrence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace eSim.Implementations.Services.Selfcare.Reference
{
    public class CountryService(IMiddlewareConsumeApi consumeApi) : ICountyService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;

        //private readonly HttpClient _http = http;
        //private readonly IHttpContextAccessor _httpContext = httpContext;

        public async Task<List<CountriesDTO>> Countries()
        {

            var Url = BusinessManager.MdwBaseURL + BusinessManager.Countries; 
            var resp = await _consumeApi.Get<List<CountriesDTO>>(Url);

            if (resp.Success)
            {
                return resp.Data;    
            }

            return null ;
        }
    }
}
