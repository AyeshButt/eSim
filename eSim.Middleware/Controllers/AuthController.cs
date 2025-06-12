using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Middleware;
using eSim.Common.StaticClasses;

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

        public IActionResult Login([FromBody] AuthDTORequest input)
        {
            var token = _authService.Authenticate(input);

            if (token == null)
                return Unauthorized(new { message =BusinessManager.InvalidLOgin });

            return Ok(new { access_token = token });
        }



    }




}
