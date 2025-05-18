using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Middleware.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Middleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketServices _ticketServices;
        public TicketController(ITicketServices ticketServices)
        {
            _ticketServices = ticketServices;
        }
        #region Generate Ticket
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketApiDto ticketDto)
        {
            try
            {
                var createdTicket = await _ticketServices.CreateTicketAsync(ticketDto);

                return Ok(new
                {
                    Status = "Success",
                    Message = "Ticket created successfully",
                    Response = createdTicket
                });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "An error occurred while creating the ticket.",
                    Error = ex.Message
                });
            }
        }
        #endregion
    }
}
