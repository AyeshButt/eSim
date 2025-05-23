using eSim.Common.StaticClasses;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Middleware.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class ReferenceController(IBundleService bundleService) : ControllerBase
    {
        private readonly IBundleService _bundleService = bundleService;


        [HttpGet]
        [Route("Countries")]
        public IActionResult GetCountries()
        {


            var subKey = User.Claims.FirstOrDefault(a => a.Type == BusinessManager.LoginSubcriberClaim)?.Value; 

            if (subKey == null)
                return Unauthorized();


            return Ok(subKey);
           // return Ok(_bundleService.GetCountries());
        }
    }
}
