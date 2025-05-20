using eSim.Implementations.Services.Middleware.Subscriber;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriberController(ISubscriberService subscriber) : ControllerBase
    {
        private readonly ISubscriberService _subscriber = subscriber;
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required.");
            }

            var exists = await _subscriber.EmailExists(email);

            if (exists)
            {
                return Ok("Email already exists.");
            }

            return Ok("Email is available.");
        }


        [HttpPost]

        public IActionResult POST(SubscriberRequestDTO input)
        {
            if (ModelState.IsValid)
            {
                var result = _subscriber.CreateSubscriber(input).GetAwaiter().GetResult();
                if (result.Success)
                {
                    return Ok(input);
                }
                else
                {
                    return Problem(result.Data);
                }
            }
            else
            {

                return BadRequest();
            }

        }
    }
}
