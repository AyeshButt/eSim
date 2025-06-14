using eSim.Infrastructure.Interfaces.Middleware.Esim;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using eSim.Infrastructure.DTOs.Esim;
using Azure;
using eSim.Common.StaticClasses;
using eSim.EF.Entities;
using System.Net.Http.Headers;
using System.Net.Http;
using Org.BouncyCastle.Ocsp;
using System.Net;
using eSim.Infrastructure.DTOs.Global;
namespace eSim.Middleware.Controllers
{
    [Route("esims")]
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

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        
        }
        #endregion

        #region History of  Esim
        [AllowAnonymous]
        [HttpGet("History")]
        public async Task<IActionResult> GetEsimHistory([FromQuery] string iccid)
        {
            var result = await _esimService.GetEsimHistoryAsync(iccid);


            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion
        
        #region EsimBundleInventory
        [AllowAnonymous]
        [HttpGet("Inventory")]
        public async Task<IActionResult> GetBundleInventory()
        {
            var result = await _esimService.GetEsimBundleInventoryAsync();

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        
        }

        #endregion
        
        #region EsimBundleInventory
        [AllowAnonymous]
        [HttpGet("EsimInstallDetail")]
        public async Task<IActionResult> GetEsimInstallDetail([FromQuery] string reference)
        {
            var result = await _esimService.GetEsimInstallDetailAsync(reference);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
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

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion
        
        #region ListBundlesAppliedToESIM
        [AllowAnonymous]
        [HttpGet("ListBundlesAppliedToESIM")]
        public async Task<IActionResult> ListBundlesAppliedToESIM([FromQuery] ListBundlesAppliedToESIMRequestDTO request)
        {
            var result = await _esimService.GetListBundlesappliedtoeSIMAsync(request);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion

        [AllowAnonymous]
        [HttpGet("{iccid}/qr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized,Type = typeof(Result<string>))]        
        [ProducesResponseType(StatusCodes.Status403Forbidden,Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(Result<string>))]
        public async Task<IActionResult> DownloadQR([FromRoute] string iccid)
        {
            var response = await _esimService.DownloadQRAsync(iccid);

            if(response.Data is not null)
            return File(response.Data, BusinessManager.ImageMediaContentType, BusinessManager.QRCode);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }
       
    }
}
