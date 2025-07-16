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
using Microsoft.EntityFrameworkCore;
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
        public async Task<Result<List<TicketsResponse>>> Get()
        {

            string URL = BusinessManager.MdwBaseURL + BusinessManager.Ticekt;

            var data = await _consumeApi.Get<List<TicketsResponse>>(URL);

            return data;

        }

        #endregion


        #region Get Ticket Type
        public async Task<Result<List<TicketTypeResponse>>> GetTicketType()
        {

            string URL = BusinessManager.MdwBaseURL + BusinessManager.TicketType;

           
            var response = await _consumeApi.Get<List<TicketTypeResponse>>(URL);

            return response;
            
        }

        #endregion



        #region create Ticket
        public async Task<Result<string>> Create(TicketRequestViewModel model)
        {
            string URL = BusinessManager.MdwBaseURL + "Ticket";
            string attachmentUrl = BusinessManager.MdwBaseURL + "ticket/attachment";
            var token = _httpContext.HttpContext?.User?.FindFirst("Token")?.Value;

            try
            {
                TicketRequest dto = new()
                {
                    TicketType = model.TicketType,
                    Subject = model.Subject,
                    Description = model.Description
                };

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var ticketResponse = await _http.PostAsJsonAsync(URL, dto);
                var ticketJson = await ticketResponse.Content.ReadAsStringAsync();
                var ticketResult = JsonConvert.DeserializeObject<Result<string>>(ticketJson);

                if (ticketResult == null || !ticketResult.Success)
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = ticketResult?.Message ?? "Ticket creation failed"
                    };
                }

               
                if (model.File != null)
                {
                    if (model.File.Length > 102400)
                    {
                        return new Result<string>
                        {
                            Success = false,
                            Message = "File size exceeds the allowed limit of 100 KB."
                        };
                    }

                        using var content = new MultipartFormDataContent();
                    content.Add(new StringContent(ticketResult.Data!), "TRN");
                    content.Add(new StreamContent(model.File.OpenReadStream()), "File", model.File.FileName);

                    var attachmentResponse = await _http.PostAsync(attachmentUrl, content);
                    var attachmentJson = await attachmentResponse.Content.ReadAsStringAsync();
                    var attachmentResult = JsonConvert.DeserializeObject<Result<string>>(attachmentJson);

                    if (!attachmentResult.Success)
                    {
                        return new Result<string>
                        {
                            Success = false,
                            Message = "Ticket created but attachment failed: " + (attachmentResult?.Message ?? "")
                        };
                    }
                }

                return new Result<string>
                {
                    Success = true,
                    Message = "Ticket created successfully",
                    Data = ticketResult.Data
                };
            }
            catch
            {
                return new Result<string>
                {
                    Success = false,
                    Message = "Failed to create ticket or upload attachment"
                };
            }
        }

        #endregion

        #region Detail
        public async Task<Result<TicketDetailDTO>> Detail(string trn)
        {
            var Url = BusinessManager.MdwBaseURL + BusinessManager.TicketDetail;

            var fulUrl = $"{Url}?trn={Uri.EscapeDataString(trn)}";

            var request = await _consumeApi.Get<TicketDetailDTO>(fulUrl);
 
    

            
            return request;
        }

        public async Task<Result<bool>> PostCommentAsync(TicketCommentRequest request)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.Ticketcomment;
            var result = await _consumeApi.Post<bool,TicketCommentRequest>(url, request);
            return result;
        }

   



        #endregion


    }
}

