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
using eSim.Common.Extensions;
using eSim.Infrastructure.DTOs;
using eSim.Infrastructure.DTOs.Middleware.Order;
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

        #region List eSIMs
        [Authorize]
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<EsimsDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListofyoureSims()
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return Unauthorized();

            var result = await _esimService.GetListofEsimsAsync(loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);

        }
        #endregion

        #region Get Esim History

        [Authorize]
        [HttpGet("{iccid}/history")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Result<GetEsimHistoryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetEsimHistory([FromRoute] string iccid)
        {
            var result = await _esimService.GetEsimHistoryAsync(iccid);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion

        #region Get eSIM Install Details

        [Authorize]
        [HttpGet("install-details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
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

            if (result.Message is not null)
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

        #region GetappliedBundlestatus
        [AllowAnonymous]
        [HttpGet("appliedBundlestatus")]
        public async Task<IActionResult> GetappliedBundlestatus([FromQuery] GetAppliedBundleStatusRequestDTO request)
        {
            //var result = await _esimService.GetAppliedBundleStatusAsync(request);

            //return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
            return Ok();
        }

        #endregion

        #region Download QR

        [AllowAnonymous]
        [HttpGet("{iccid}/qr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<string>))]
        public async Task<IActionResult> DownloadQR([FromRoute] string iccid)
        {
            var response = await _esimService.DownloadQRAsync(iccid);

            if (response.Data is not null)
                return File(response.Data, BusinessManager.ImageMediaContentType, BusinessManager.QRCode);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion

        #region Apply bundle to an existing esim

        [AllowAnonymous]
        [HttpPost("apply-bundle-existing-esim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<string>))]
        public async Task<IActionResult> ApplyBundleToExistingEsim(ApplyBundleToExistingEsimRequest input)
        {
            Result<ApplyBundleToEsimResponse> response = new();

            var loggedUser = "c595c0f5-9a8b-4cec-9733-08dda9a3fe55";

            if (loggedUser == null)
                return Unauthorized();

            response = await _esimService.ApplyBundleToExistingEsimAsync(input, loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion

        #region Apply bundle to a new esim

        [AllowAnonymous]
        [HttpPost("apply-bundle-new-esim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<ApplyBundleToEsimResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<ApplyBundleToEsimResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<ApplyBundleToEsimResponse>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<ApplyBundleToEsimResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<ApplyBundleToEsimResponse>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<ApplyBundleToEsimResponse>))]
        public async Task<IActionResult> ApplyBundleToEsim(ApplyBundleToEsimRequest input)
        {
            //var loggedUser = User.SubscriberId();
            var loggedUser = "c595c0f5-9a8b-4cec-9733-08dda9a3fe55";

            if (loggedUser == null)
                return Unauthorized();

            var response = await _esimService.ApplyBundleToEsimAsync(input, loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion

        #region Get esim details

        [AllowAnonymous]
        [HttpGet("details/{iccid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<GetEsimDetailsResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]

        public async Task<IActionResult> GetEsimDetails([FromRoute] string iccid, [FromQuery] string? additionalFields)
        {
            var response = await _esimService.GetEsimDetailsAsync(iccid, additionalFields);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion
    }
}
