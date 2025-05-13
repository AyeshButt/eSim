using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using eSim.EF.Entities;

namespace eSim.Middleware.Controllers
{
    [Route("api/[controller]")]
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
        public IActionResult Login([FromBody] Login model)
        {
            var token = _authService.Authenticate(model.Username, model.Password);

            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
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
