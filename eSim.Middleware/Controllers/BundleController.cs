using eSim.Common.StaticClasses;
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


        private readonly IBundleService _bundle;
        public BundleController(IBundleService bundle)
        {
            _bundle = bundle;
        }
        #region GetBundles

        [AllowAnonymous]
        [HttpPost("Bundles")] 
 
        public async Task<IActionResult> GetBundleCatalogue([FromBody] BundleRequest input)
        {            
            var result = await _bundle.GetBundlesAsync(input);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        
        }

        #endregion

        #region GetBundleDetailsFromCatalogue
        [AllowAnonymous]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetBundleDetailsFromCatalogue(string name)
        {
            var result = await _bundle.GetBundleDetailsAsync(name);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }
        #endregion

    }
}