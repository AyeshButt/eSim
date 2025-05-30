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
            if (request == null) return BadRequest("Request body is missing.");

            var result = await _bundleService.GetBundlesAsync(request.Region);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(result);
        }


        #endregion


        [AllowAnonymous]
        [HttpPost("GetByName")]
        public async Task<IActionResult> GetBundleDetails([FromBody] BundleNameDTO NameDTO)
        {
            if (string.IsNullOrWhiteSpace(NameDTO.Name))
                return BadRequest("Name is required.");

            var result = await _bundleService.GetBundleDetailAsync(NameDTO.Name);

            if (!result.Success || result.Data == null)
                return NotFound(new { message = result.Message });

            return Ok(result );
        }





    }
}