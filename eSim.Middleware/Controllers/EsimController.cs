using eSim.Infrastructure.Interfaces.Middleware.Esim;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
namespace eSim.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EsimController : ControllerBase
    {
        private readonly IEsimService _esimService;
        public EsimController(IEsimService esimService)
        {
            _esimService = esimService;
        }
        #region ListofEsim
        [AllowAnonymous]
        [HttpGet("List")]
        public async Task<IActionResult>  GetListofyoureSims()
        {
            var result = await _esimService.GetListofEsimsAsync();

            if (!result.Success)
            {
                return BadRequest( result);
            }

            return Ok(result);
        }
        #endregion
        #region History of  Esim
        [AllowAnonymous]
        [HttpGet("History/{iccid}")]
        public async Task<IActionResult> GetEsimHistory([FromQuery] string iccid)
        {
            var result = await _esimService.GetEsimHistoryAsync(iccid);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region EsimBundleInventory
        [AllowAnonymous]
        [HttpGet("Inventory")]
        public async Task<IActionResult> GetBundleInventory()
        {
            var result = await _esimService.GetEsimBundleInventoryAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

    }
}
