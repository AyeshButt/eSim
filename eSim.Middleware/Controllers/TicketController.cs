using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Middleware.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eSim.Middleware.Controllers
{
    [Route("[controller]")]
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
        public IActionResult Get()
        {
            /// still need improvement / like filters options
            return Ok(_ticketServices.Tickets());

        }

        #endregion


        #region Generate Ticket
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> POST([FromBody] TicketRequestDTO ticketDto)
        {

            if (ModelState.IsValid)
            {

                var result = await _ticketServices.CreateTicketAsync(ticketDto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }


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


    }
}
