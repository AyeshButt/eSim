using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BundleController : ControllerBase
    {
        private readonly IBundleService _bundleService;
         
        public BundleController(IBundleService bundleService)
        {
            _bundleService = bundleService;
        }
        #region GetBundles


        [AllowAnonymous]
        [HttpPost("GetByRegion")]
        public async Task<IActionResult> GetBundles([FromBody] RegionDTO request)
        {
            
            var result = await _bundleService.GetBundlesAsync(request);

            if (!result.Success)
                return NotFound( result );

            return Ok(result);
        }


        #endregion


        [AllowAnonymous]
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetBundleDetails([FromQuery] string name)
        {


            var result = await _bundleService.GetBundleDetailAsync(name);

            if (!result.Success || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }






    }
}