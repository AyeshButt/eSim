using eSim.Infrastructure.DTOs.Admin.Inventory;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Admin.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace eSim.Admin.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventory _inventory;
        private readonly IClient _client;

        public InventoryController(IInventory inventory, IClient client)
        {
            _inventory = inventory;
            _client = client;
        }
        [Authorize(Policy="Inventory:view")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new AdminInventoryViewModel();
            
            var inventory = await _inventory.GetInventoryAsync();
            
            model.Inventory = inventory.OrderByDescending(u=>u.CreatedDate).ToList();

            var clients = await _client.GetAllClientsAsync();

            if (inventory is null || clients is null)
                return View(model);

            ViewBag.Clients = clients.Select(u => new SelectListItem() {Text =  u.Name, Value=u.Id.ToString()}).ToList();
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetFilteredInventory(AdminInventoryFilterDTO input)
        {
            var inventory = await _inventory.GetInventoryAsync();

            List<AdminInventoryDTO> filteredList = FilterInventory(inventory, input).OrderByDescending(u=>u.CreatedDate).ToList();

            return PartialView("_InventoryListPartial", filteredList);
        }
        [HttpGet]
        public async Task<IActionResult> GetClientSubscribers(string clientId)
        {
            var subscribers = await _inventory.GetClientSubscribersAsync(clientId);

            if (subscribers is null)
                return Json(new List<SelectListItem>());

            var model = subscribers.Select(u => new SelectListItem() { Text = $"{u.FirstName} {u.LastName}", Value = u.Id.ToString()}).ToList();

            return Json(model);
        }

        #region Filter inventory list
        private IQueryable<AdminInventoryDTO> FilterInventory(IQueryable<AdminInventoryDTO> inventory, AdminInventoryFilterDTO input)
        {
            if (input.Client is not null)
            {
                inventory = inventory.Where(u => u.ClientId ==Guid.Parse(input.Client));
            }
            if (input.Subscriber is not null)
            {
                inventory = inventory.Where(u => u.SubscriberId == Guid.Parse(input.Subscriber));
            }
            if (input.Date is not null)
            {
                var splitDate = input.Date.Split("to");

                if (splitDate.Length > 1)
                {
                    var from = DateTime.Parse(splitDate[0]);
                    var toDateOnly = DateTime.Parse(splitDate[1]);

                    var to = toDateOnly.Date.AddDays(1).AddSeconds(-1); // Sets to 23:59:59

                    inventory = inventory.Where(u => u.CreatedDate >= from && u.CreatedDate <=to);

                }
                else
                {
                    inventory = inventory.Where(u => u.CreatedDate >= DateTime.Parse(input.Date));
                }
            }
            
            return inventory;
        }
        #endregion
    }
}
