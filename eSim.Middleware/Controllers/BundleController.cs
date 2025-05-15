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

        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> GetBundles()
        {
            var result = await _bundleService.GetBundlesAsync();

            if (!result.Success || result.Data == null)
            {
                return NotFound(" No bundles found or API call failed.");
            }

            if (result.Data.bundles == null || !result.Data.bundles.Any())
            {
                return Ok(new
                {
                    message = " Request successful, but now you have null bundle.",
                    //bundles = result.Data.bundles,
                    //pageCount = result.Data.pageCount,
                    //rows = result.Data.rows,
                    //pageSize = result.Data.pageSize
                });
            }

            return Ok(result.Data);
        }
    }
}
