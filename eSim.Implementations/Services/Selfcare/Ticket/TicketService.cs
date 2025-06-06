using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Selfcare.Ticket;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Ticket;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace eSim.Implementations.Services.Selfcare.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMiddlewareConsumeApi _consumeApi;

        public TicketService(HttpClient http, IHttpContextAccessor httpContext, IMiddlewareConsumeApi consumeApi)
        {

            _http = http;
            _httpContext = httpContext;
            _consumeApi = consumeApi;
        }

        #region GetTicket List 
        public async Task<Result<List<TicketsResponseDTO>>> Get()
        {

            string URL = BusinessManager.MdwBaseURL + BusinessManager.Ticekt;

            var data = await _consumeApi.Get<List<TicketsResponseDTO>>(URL);

            return data;

        }

        #endregion


        #region Get Ticket Type
        public async Task<Result<List<TicketTypeResponseDTO>>> GetTicketType()
        {

            string URL = BusinessManager.MdwBaseURL + BusinessManager.TicketType;

           
            var response = await _consumeApi.Get<List<TicketTypeResponseDTO>>(URL);

            return response;
            
        }

        #endregion



        #region create Ticket
        public async Task<Result<string>> Create(TicketRequestViewModel model)
        {
            string URL = BusinessManager.MdwBaseURL + "Ticket";
            var token = _httpContext.HttpContext?.User?.FindFirst("Token")?.Value;


            try
            {
                TicketRequestDTO dto = new()
                {
                    TicketType = model.TicketType,
                    Subject = model.Subject,
                    Description = model.Description
                };

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = await _http.PostAsJsonAsync(URL, dto);
                var json = await req.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<Result<string>>(json);
                return resp;
            }
            catch (Exception ex)
            {
                return new Result<string>() { Success = false, Message = "Failed to Open New Ticket" };
            }

        }

        public async Task<Result<TicketDetailDTO>> Detail(string trn)
        {
            var Url = BusinessManager.MdwBaseURL + BusinessManager.TicketDetail;

            var fulUrl = $"{Url}?trn={Uri.EscapeDataString(trn)}";

            var request = await _consumeApi.Get<TicketDetailDTO>(fulUrl);
            
            return request;
        }

        #endregion


    }
}

