using eSim.Common.Extensions;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.Middleware.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Middleware.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventory _inventory;

        public InventoryController(IInventory inventory)
        {
            _inventory = inventory;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<GetBundleInventoryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetBundleInventory()
        {
            var response = await _inventory.GetBundleInventoryAsync();

            return response.Success ? StatusCode(StatusCodes.Status200OK, response) : StatusCode(StatusCodes.Status400BadRequest, response);
        }

        [HttpGet("subscriber")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<SubscriberInventoryResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetSubscriberInventory()
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return StatusCode(StatusCodes.Status401Unauthorized, new Result<string> { Success = false, Message = string.Empty });

            var response = await _inventory.GetSubscriberInventoryResponse(loggedUser);

            return response.Success ? StatusCode(StatusCodes.Status200OK,response) : StatusCode(StatusCodes.Status400BadRequest,response);
        }


        [HttpPost("refund")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> RefundBundle(RefundBundleDataBaseRequest model)
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return StatusCode(StatusCodes.Status401Unauthorized, new Result<string> { Success = false, Message = string.Empty });

            var response = await _inventory.RefundBundleAsync(model, loggedUser);   

            return response.Success ? StatusCode(StatusCodes.Status200OK, response) : StatusCode(StatusCodes.Status400BadRequest, response);
        }
    }
}
