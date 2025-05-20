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
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Selfcare.Ticket;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace eSim.Implementations.Services.Selfcare.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient _http;

        public TicketService(HttpClient http )
        {
            
            _http = http;
        }

        #region GetTicket List 
        public async Task<Result<List<TicketsResponseDTO>>> Get()
        {
            string URL = BusinessManager.MdwBaseURL + "Ticket";

            _http.DefaultRequestHeaders.Authorization   = new AuthenticationHeaderValue("Bearer", BusinessManager.AuthToken);

            var resp = await _http.GetAsync(URL);

            if (resp.IsSuccessStatusCode) 
            {
                var result = JsonConvert.DeserializeObject<Result<List<TicketsResponseDTO>>>(await resp.Content.ReadAsStringAsync());

                return result;
            }

            return new Result<List<TicketsResponseDTO>>() { Success = false, Message = "Faild to fetch data" };
        }

        #endregion


        #region Get Ticket Type
        public async Task<Result<List<TicketTypeResponseDTO>>> GetTicketType()
        {
            string URL = BusinessManager.MdwBaseURL + "Ticket/Types";
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BusinessManager.AuthToken);

            var req = await _http.GetAsync(URL);
            if (req.IsSuccessStatusCode) 
            { 
                var result = JsonConvert.DeserializeObject<Result<List<TicketTypeResponseDTO>>>(await req.Content.ReadAsStringAsync());
                return result;
            }
            return new Result<List<TicketTypeResponseDTO>>() { Success = false, Message = "Faild to fetch data" };
        }

        #endregion



        #region create Ticket
        public async Task<Result<string>> Create(TicketRequestDTO dto)
        {
            string URL = BusinessManager.MdwBaseURL + "Ticket";
            try
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BusinessManager.AuthToken);

                var req = await _http.PostAsJsonAsync(URL, dto);
                var json = await req.Content.ReadAsStringAsync();
                Console.WriteLine(json);
                var resp = JsonConvert.DeserializeObject<Result<string>>(json);
                return resp;
            }
            catch (Exception ex) 
            {  
                return new Result<string>() { Success = false, Message = "Failed to Open New Ticket" };
            }

        }

        #endregion


    }
}

