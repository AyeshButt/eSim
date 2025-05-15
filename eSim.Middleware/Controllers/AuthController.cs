using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Middleware;

namespace eSim.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IExternalApiService _externalApiService;

        public AuthController(IAuthService authService, IExternalApiService externalApiService)
        {
            _authService = authService;
            _externalApiService = externalApiService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthDTO model)
        {
            var token = _authService.Authenticate(model);

            if (token == null)
                return Unauthorized(new { message = "Invalid-credentials" });

            return Ok(new { access_token = token });
        }

        //[Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok($"Hello {User.Identity?.Name}, you are authenticated.");
        }
        // No JWT required
        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("This is a public endpoint.");
        }

        [AllowAnonymous]
        [HttpGet("public1")]
        public IActionResult Public1()
    {
            var result = _externalApiService.GetOrders();

            if (string.IsNullOrEmpty(result))
                return StatusCode(500, "Failed to fetch data from external API.");

            return Ok(result);
        }

    }
}
