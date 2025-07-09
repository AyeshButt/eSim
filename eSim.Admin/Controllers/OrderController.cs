using eSim.Implementations.Services.Admin.Order;
using eSim.Implementations.Services.Middleware.Order;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.Admin.Order;
using eSim.Infrastructure.Interfaces.Middleware.Order;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAdminOrder _Order;
        public OrderController(IAdminOrder Order)
        {
            _Order = Order;
        }
        public async Task<IActionResult> Index(ListOrderRequest request)
        {
            var result = await _Order.GetOrdersFromDbAsync(request);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(new ListOrderResponse());
            }

            return View(result.Data);
        }


    
    [HttpGet]
        public async Task<IActionResult> Detail(string orderReferenceId)
        {
            var result = await _Order.GetOrderDetailAsync(orderReferenceId);

            if (!result.Success || result.Data == null)
            {

                ViewBag.Error = result.Message ?? "Something went wrong.";
                return View(new GetOrderDetailResponse());
            }

            return View(result.Data);
        }

    } 
}
