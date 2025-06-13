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

        [HttpPost]
       
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<CreateOrderResponse>))]
       
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest input)
        {
            var loggedUser = User.SubscriberId();

            if (loggedUser is null)
                return StatusCode(StatusCodes.Status401Unauthorized, new Result<string> { Success = false, Message = string.Empty });

            var response = await _order.CreateOrderAsync(input, loggedUser);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }

        [AllowAnonymous]
        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<ListOrderResponse>))]
        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> ListOrder([FromQuery] ListOrderRequest input)
        {
            var response = await _order.ListOrderAsync(input);
            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        
        }

        [AllowAnonymous]
        [HttpGet("detail/{orderReferenceId}")]
        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<GetOrderDetailResponse>))]

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<string>))]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetOrderDetail([FromRoute] string orderReferenceId)
        {
            var response = await _order.GetOrderDetailAsync(orderReferenceId);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(response.StatusCode), response);
        }
    }
}
