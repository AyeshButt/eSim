using System.Security.Claims;
using eSim.Common.Extensions;
using eSim.Common.StaticClasses;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Middleware.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eSim.Middleware.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketServices _ticketServices;
        public TicketController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }


        #region Generate Ticket

        [HttpGet]
        [Route("List")]
        public IActionResult Get()
        {
        
            return Ok(_ticketServices.Tickets());

        }

        #endregion


        #region Generate Ticket

        [HttpPost]
        public async Task<IActionResult> POST([FromBody] TicketRequest ticketDto)
        {

            var result = await _ticketServices.CreateTicketAsync(ticketDto);
            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);

        }

        
        #endregion


        #region Ticket Type
        [HttpGet]
        [Route("Types")]
        public IActionResult GET()
        {
            return Ok(_ticketServices.GetTicketType());
        }

        #endregion
        #region TicketAttachment
        [AllowAnonymous]
        [HttpPost("Attachment")]
        public async Task<IActionResult> UploadAttachment([FromForm] TicketAttachmentRequest dto)
        {
          
            var result = await _ticketServices.UploadAttachmentAsync(dto);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion
        #region TicketDetail


        [HttpGet("detail")]
        public async Task<IActionResult> GetTicketDetail(string trn)
        {
            var result = await _ticketServices.GetTicketDetailAsync(trn);
            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }
        
        #endregion
        [HttpPost("comment")]
        public async Task<IActionResult> AddComment([FromBody] TicketCommentRequest input)
        {
      

            var loggedUser = User.SubscriberId();
            
            if (loggedUser is null)
                return StatusCode(StatusCodes.Status401Unauthorized, new Result<string> { Success = false ,Message = string.Empty});

            var result = await _ticketServices.AddCommentAsync(input, loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }


    }
}
