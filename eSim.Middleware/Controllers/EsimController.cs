using eSim.Infrastructure.Interfaces.Middleware.Esim;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using eSim.Infrastructure.DTOs.Esim;
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

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }
        #endregion
        #region History of  Esim
        [AllowAnonymous]
        [HttpGet("History")]
        public async Task<IActionResult> GetEsimHistory([FromQuery] string iccid)
        {
            var result = await _esimService.GetEsimHistoryAsync(iccid);

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        #endregion

        #region EsimBundleInventory
        [AllowAnonymous]
        [HttpGet("Inventory")]
        public async Task<IActionResult> GetBundleInventory()
        {
            var result = await _esimService.GetEsimBundleInventoryAsync();

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        #endregion
        #region EsimBundleInventory
        [AllowAnonymous]
        [HttpGet("EsimInstallDetail")]
        public async Task<IActionResult> GetEsimInstallDetail([FromQuery] string reference)
        {
            var result = await _esimService.GetEsimInstallDetailAsync(reference);

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        #endregion
        #region EsimBundleInventory
        [AllowAnonymous]
        [HttpGet("eSIMCompatibility")]
        public async Task<IActionResult> CheckeSIMAndBundleCompatibility([FromQuery] EsimCompatibilityRequestDto request)
        {
            var result = await _esimService.CheckeSIMandBundleCompatibilityAsync(request);

            if (result.Message is not  null)
                return StatusCode(StatusCodes.Status403Forbidden, result);

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        #endregion
        #region ListBundlesAppliedToESIM
        [AllowAnonymous]
        [HttpGet("ListBundlesAppliedToESIM")]
        public async Task<IActionResult> ListBundlesAppliedToESIM([FromQuery] ListBundlesAppliedToESIMRequestDTO request)
        {
            var result = await _esimService.GetListBundlesappliedtoeSIMAsync(request);

         

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

#endregion

    }
}
