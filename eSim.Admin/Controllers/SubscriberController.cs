
using eSim.Common.Enums;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Admin.Inventory;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Subscribers;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Admin.Esim;
using eSim.Infrastructure.Interfaces.Admin.Inventory;
using eSim.Infrastructure.Interfaces.Admin.Order;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace eSim.Admin.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly ISubscriberService _subscriber;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClient _client;
        private readonly IAdminOrder _adminOrder;
        private readonly IInventory _inventory;
        private readonly IEsims _esim;

        public SubscriberController(ISubscriberService subscriber, UserManager<ApplicationUser> userManager, IClient client, IAdminOrder adminOrder, IInventory inventory, IEsims esim)
        {
            _subscriber = subscriber;
            _userManager = userManager;
            _client = client;
            _adminOrder = adminOrder;
            _inventory = inventory;
            _esim = esim;
        }

        [HttpGet]
        public async Task<IActionResult> Index(SubscribersResponseViewModel input)
        {
            SubscribersResponseViewModel model = new();

            var clients = await _client.GetAllClientsAsync();
            var subScribers = await _subscriber.GetClient_SubscribersListAsync();

            if (subScribers is null || clients is null)
                return View(model);

            model.SubscribersResponse = FilterSubscribers(subScribers, input).ToList();


            ViewBag.Clients = clients.Select(u => new ClientDTO() { Name = u.Name, Id = u.Id }).ToList();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> GetClientSubscribers(string clientId)
        {
            var subscribers = await _subscriber.GetClient_SubscribersListByID(clientId);

            if (subscribers is null)
                return Json(new List<SelectListItem>());

            var model = subscribers.Select(u => new SelectListItem() { Text = u.Email, Value = u.Id.ToString() }).ToList();

            return Json(model);
        }

        private IQueryable<SubscribersResponseDTO> FilterSubscribers(IQueryable<SubscribersResponseDTO> inventory, SubscribersResponseViewModel input)
        {
            if (input == null) return inventory;

            // Date Range Filter
            if (!string.IsNullOrWhiteSpace(input.DateRange))
            {
                var dates = input.DateRange.Split(new[] { " to " }, StringSplitOptions.RemoveEmptyEntries);

                if (dates.Length == 2 &&
                    DateTime.TryParse(dates[0], out var fromDate) &&
                    DateTime.TryParse(dates[1], out var toDate))
                {
                    inventory = inventory.Where(u => u.CreatedAt >= fromDate && u.CreatedAt <= toDate);
                }
            }

            // Client Filter
            if (!string.IsNullOrWhiteSpace(input.Client) && Guid.TryParse(input.Client, out var clientId))
            {
                inventory = inventory.Where(u => u.ClientId == clientId);
            }

            // Subscriber Filter
            if (!string.IsNullOrWhiteSpace(input.Subscriber) && Guid.TryParse(input.Subscriber, out var subscriberId))
            {
                inventory = inventory.Where(u => u.Id == subscriberId);
            }

            return inventory; // Don't forget to return the filtered query
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string Id)
        {
            Guid.TryParse(Id, out Guid GuidID);
            var request = await _subscriber.GetSubscriberDetailAsync(GuidID);
            var subscriberDetail = request.Data;
            return View(subscriberDetail);
        }

        [HttpPost]
        public async Task<IActionResult> TabPartialViews(string tab, string subId)
        {
            Guid.TryParse(subId, out Guid GuidId);
            switch (tab)
            {
                case "order":

                    var request = await _adminOrder.GetOrderBySubscriberId(subId);
                    return PartialView("_OrdersPartialView", request);

                case "inventory":

                    var CompleteInventory = await _inventory.GetInventoryAsync();
                    var SubscriberInventory = CompleteInventory.Where(u => u.SubscriberId == GuidId).ToList();
                    return PartialView("_InventoryPartialView", SubscriberInventory);
                
                case "esim":
                    var EsimList = await _esim.GetAllAsync(subId);
                    return PartialView("_EsimPartialView", EsimList);
                
                default:
                    var TicketList = await _esim.GetAllTicketAsync(subId);
                    return PartialView("_TicketsPartialView", TicketList);
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    IQueryable<SubscriberDTO> subscriberList = null!;

        //    var user = await _userManager.GetUserAsync(User);

        //    if (user == null)
        //        return RedirectToAction("Error", "Account");

        //    if (user.UserType == (int)AspNetUsersTypeEnum.Client)
        //    {
        //        subscriberList = await _subscriber.GetClient_SubscribersListAsync(user.Id);

        //    }
        //    else if (user.UserType == (int)AspNetUsersTypeEnum.Subclient)
        //    {
        //        subscriberList = await _subscriber.GetClient_SubscribersListAsync(user.ParentId ?? string.Empty);
        //    }
        //    else
        //    {
        //        //for superadmin and other accounts
        //        subscriberList = Enumerable.Empty<SubscriberDTO>().AsQueryable();
        //    }

        //    return View(subscriberList.ToList());

        //}

    }
}

