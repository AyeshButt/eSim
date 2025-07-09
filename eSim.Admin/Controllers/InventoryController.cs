using eSim.Infrastructure.DTOs.Admin.Inventory;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Admin.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        public async Task<IActionResult> Index(AdminInventoryViewModel input)
        {
            var model = new AdminInventoryViewModel();
            
            var inventory = await _inventory.GetInventoryAsync();
            var clients = await _client.GetAllClientsAsync();

            if (inventory is null || clients is null)
                return View(model);

            model.Inventory = FilterInventory(inventory,input).ToList();

            ViewBag.Clients = clients.Select(u => new SelectListItem() {Text =  u.Name, Value=u.Id.ToString()}).ToList();
            
            return View(model);
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
        private IQueryable<AdminInventoryDTO> FilterInventory(IQueryable<AdminInventoryDTO> inventory,AdminInventoryViewModel input)
        {
            if (input.Client is not null)
            {
                inventory = inventory.Where(u => u.ClientId ==Guid.Parse(input.Client));
            }
            if (input.Subscriber is not null)
            {
                inventory = inventory.Where(u => u.SubscriberId == Guid.Parse(input.Subscriber));
            }
            if (input.From is not null)
            {
                inventory = inventory.Where(u => u.CreatedDate >= input.From);
            }
            if (input.To is not null)
            {
                inventory = inventory.Where(u => u.CreatedDate <= input.To);
            }

            return inventory;
        }
        #endregion
    }
}
