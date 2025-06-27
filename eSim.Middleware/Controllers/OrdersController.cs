using eSim.Common.Extensions;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.Middleware.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Raven.Client.Linq.LinqPathProvider;

namespace eSim.Middleware.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrder _order;

        public OrdersController(IOrder order)
        {
            _order = order;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<CreateOrderResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<string>))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest input)
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return Unauthorized();

            var model = new CreateOrderDTO()
            {
                Type = BusinessManager.Transaction,
                Order = input.Order,
            };

            var response = await _order.CreateOrderAsync(model, loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<ListOrderResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<string>))]
        public async Task<IActionResult> ListOrder([FromQuery] ListOrderRequest input)
        {
            var response = await _order.ListOrderAsync(input);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        [Authorize]
        [HttpGet("detail/{orderReferenceId}")]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<GetOrderDetailResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(Result<string>))]
        public async Task<IActionResult> GetOrderDetail([FromRoute] string orderReferenceId)
        {
            var response = await _order.GetOrderDetailAsync(orderReferenceId);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }
    }
}
