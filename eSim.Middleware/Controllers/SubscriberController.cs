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
