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

           // request.Region = string.IsNullOrEmpty(request.Region) ? "Asia" : request.Region;


            var result = await _bundleService.GetBundlesAsync(request.Region);

            if (!result.Success || result.Data == null)
            {
                return NotFound("No bundles found or API call failed.");
            }

            if (result.Data.bundles == null || !result.Data.bundles.Any())
            {
                return Ok(new { message = "No bundles found for the specified region." });
            }

            return Ok(result.Data);
        }

        #endregion



        [AllowAnonymous]
        [HttpPost("GetByName")]
        public async Task<IActionResult> GetBundleDetails([FromBody] BundleNameDTO NameDTO)
        {
            if (string.IsNullOrWhiteSpace(NameDTO.Name))
            {
                return BadRequest("The 'name' parameter is required and cannot be empty.");
            }

            var result = await _bundleService.GetBundleDetailAsync(NameDTO.Name);

            if (result is { Success: false } or { Data: null })
            {
                return NotFound("No bundle found with the specified name, or the API call failed.");
            }

            return Ok(result.Data);
        }


      

    }
}