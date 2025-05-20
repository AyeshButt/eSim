using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.Interfaces.Admin.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eSim.Admin.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClient _client;

        public ClientController(IClient client)
        {
            _client = client;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [Authorize(Policy = "Clients:view")]

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clientList = await _client.GetAllClientsAsync();

            return View(clientList.ToList());
        }
        [Authorize(Policy = "Clients:create")]

        [HttpGet]
        public IActionResult CreateClient()
        {
            return View(new ClientDTO());
        }
        [Authorize(Policy = "Clients:create")]

        [HttpPost]
        public async Task<IActionResult> CreateClient(ClientDTO input)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CreateClient");
            }

            input.CreatedBy = input.ModifiedBy =  User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            var client = await _client.CreateClientAsync(input);

            if (!client.Success)
            {
                TempData["ClientError"] = client.Message;
                return RedirectToAction("CreateClient");

            }

            TempData["ClientCreated"] = BusinessManager.ClientCreated;

            return RedirectToAction("Index");

        }
        [Authorize(Policy = "Clients:edit")]

        [HttpGet]
        public async Task<IActionResult> UpdateClient(string id)
        {
            if (!Guid.TryParse(id,out Guid parseId))
            {
                return RedirectToAction("Error","Home");
            }

            var client = await _client.GetClientAsync(parseId);

            if (!client.Success)
            {
                TempData["ClientNotFound"] = BusinessManager.ClientNotFound;
                return RedirectToAction("Index");
            }

            return View(client.Data);
        }
        [HttpPost]
        [Authorize(Policy = "Clients:edit")]

        public async Task<IActionResult> UpdateClient(ClientDTO input)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("UpdateClient", new { id = input.Id });
            }

            input.ModifiedBy = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            var client = await _client.UpdateClientAsync(input);

            if (!client.Success && client.Data is not null)
            {
                TempData["ClientError"] = client.Data;

                return RedirectToAction("UpdateClient", new { id = input.Id });
            }

            if (!client.Success)
            {
                TempData["ClientNotFound"] = BusinessManager.ClientNotFound;

                return RedirectToAction("UpdateClient", new { id = input.Id });
            }

            TempData["ClientUpdated"] = BusinessManager.ClientUpdated;

            return RedirectToAction("Index");

        }

        [HttpGet]
        [Authorize(Policy = "Clients:disable")]

        public async Task<IActionResult> DisableClient(string id, bool enabled)
        {

            if (!Guid.TryParse(id, out Guid parseId))
            {
                return RedirectToAction("Error", "Home");
            }

            var client = await _client.DisableClientAsync(parseId);
            
            if (!client.Success && client.Data is not null)
            {
                TempData["ClientError"] = client.Data;

                return RedirectToAction("Index");
            }

            if (!client.Success)
            {
                TempData["ClientNotFound"] = BusinessManager.ClientNotFound;

                return RedirectToAction("Index");
            }

            return Json(new { success = true, id = id, enabled = enabled });
        }
    }
}
