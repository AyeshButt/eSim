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
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace eSim.Middleware.Controllers
{
    [Route("esims")]
    [ApiController]
    public class EsimController : ControllerBase
    {
        private readonly IEsimService _esim;

        public EsimController(IEsimService esim)
        {
            _esim = esim;
        }

        #region List esims

        [Authorize]
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<EsimsDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<>))]
        public async Task<IActionResult> GetListofyoureSims()
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return Unauthorized();

            var result = await _esim.GetListofEsimsAsync(loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);

        }

        #endregion

        #region Get esim details

        [Authorize]
        [HttpGet("details/{iccid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<GetEsimDetailsResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<>))]

        public async Task<IActionResult> GetEsimDetails([FromRoute] string iccid, [FromQuery] string? additionalFields)
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return Unauthorized();

            var response = await _esim.GetEsimDetailsAsync(iccid, additionalFields,loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion

        #region Get esim history

        [Authorize]
        [HttpGet("{iccid}/history")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Result<GetEsimHistoryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<>))]
        public async Task<IActionResult> GetEsimHistory([FromRoute] string iccid)
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return Unauthorized();

            var result = await _esim.GetEsimHistoryAsync(iccid,loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion

        #region Get esim applied bundles

        [Authorize]
        [HttpGet("{iccid}/bundles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<ListBundlesAppliedToEsimResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<>))]
        public async Task<IActionResult> ListBundlesAppliedToEsim([FromRoute] string iccid,[FromQuery] ListBundlesAppliedToEsimRequest input)
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return Unauthorized();

            var result = await _esim.GetListBundlesAppliedToEsimAsync(iccid,input,loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion

        #region Get eSIM installation details

        [Authorize]
        [HttpGet("install-details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<>))]
        public async Task<IActionResult> GetEsimInstallDetail([FromQuery] string reference)
        {
            var result = await _esim.GetEsimInstallDetailAsync(reference);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion

        #region Esim bundle inventory
        [AllowAnonymous]
        [HttpGet("eSIMCompatibility")]
        public async Task<IActionResult> CheckeSIMAndBundleCompatibility([FromQuery] EsimCompatibilityRequestDto request)
        {
            var result = await _esim.CheckeSIMandBundleCompatibilityAsync(request);

            if (result.Message is not null)
                return StatusCode(StatusCodes.Status403Forbidden, result);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion

        #region Get applied bundle status
        [AllowAnonymous]
        [HttpGet("appliedBundlestatus")]
        public async Task<IActionResult> GetappliedBundlestatus([FromQuery] GetAppliedBundleStatusRequestDTO request)
        {
            //var result = await _esim.GetAppliedBundleStatusAsync(request);

            //return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
            return Ok();
        }

        #endregion

        #region Download qr

        [AllowAnonymous]
        [HttpGet("{iccid}/qr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<string>))]
        public async Task<IActionResult> DownloadQR([FromRoute] string iccid)
        {
            var response = await _esim.DownloadQRAsync(iccid);
            
            if (response.Data is not null)
                return File(response.Data, BusinessManager.ImageMediaContentType, $"{BusinessManager.QRCode}_{iccid}.png");

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion

        #region Apply bundle to an existing esim

        [Authorize]
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

            var loggedUser = User.SubscriberId();
            
            if (loggedUser == null)
                return Unauthorized();

            response = await _esim.ApplyBundleToExistingEsimAsync(input, loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion

        #region Apply bundle to a new esim

        [Authorize]
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
            var loggedUser = User.SubscriberId();

            if (loggedUser == null)
                return Unauthorized();

            var response = await _esim.ApplyBundleToEsimAsync(input, loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        #endregion

    }
}
