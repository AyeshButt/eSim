using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using eSim.EF.Entities;
namespace eSim.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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



    }
}
