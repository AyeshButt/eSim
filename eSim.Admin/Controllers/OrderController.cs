using eSim.EF.Entities;
using eSim.Implementations.Services.Admin.Order;
using eSim.Implementations.Services.Middleware.Order;
using eSim.Infrastructure.DTOs.Admin.order;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.Admin.Order;
using eSim.Infrastructure.Interfaces.Middleware.Order;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy ="Orders:view")]
        public async Task<IActionResult> Index(OrderViewModel request
            )
        {
            var orderFilter = new ListOrderRequest();

            if (!string.IsNullOrEmpty(request.Date))
            {
                var parts = request.Date.Split(" to ");
                if (parts.Length == 2 &&
                    DateTime.TryParse(parts[0], out var fromDate) &&
                    DateTime.TryParse(parts[1], out var toDate))
                {
                    orderFilter.From = fromDate;
                    orderFilter.To = toDate;
                }
            }
          
            var result = await _Order.GetOrdersFromDbAsync(orderFilter);
            var filteredOrders = result.Data.Orders;

            if (!string.IsNullOrWhiteSpace(request.OrderReference))
            {
                filteredOrders = filteredOrders
                    .Where(o => o.OrderReference.Equals(request.OrderReference, StringComparison.OrdinalIgnoreCase)
)
                    .ToList();
            }
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(new OrderViewModel());
            }
            var viewModel = new OrderViewModel
            {
                Orders = filteredOrders,
                PageCount = result.Data.PageCount,
                PageSize = result.Data.PageSize,
                Rows = result.Data.Rows,
                Date = request.Date,
                OrderReference = request.OrderReference,
                
              

            };

            return View(viewModel);
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
