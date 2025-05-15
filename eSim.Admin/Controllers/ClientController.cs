using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.Interfaces.Admin.Client;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clientList = await _client.GetAllClientsAsync();

            return View(clientList.ToList());
        }
        [HttpGet]
        public IActionResult CreateClient()
        {
            return View(new ClientDTO());
        }
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
                TempData["ClientError"] = client.Data;
                return RedirectToAction("CreateClient");

            }

            TempData["ClientCreated"] = BusinessManager.ClientCreated;

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> UpdateClient(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Index");
            }

            var client = await _client.GetClientAsync(id);

            if (!client.Success)
            {
                TempData["ClientNotFound"] = BusinessManager.ClientNotFound;
                return RedirectToAction("Index");
            }

            return View(client.Data);
        }
        [HttpPost]
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
        public async Task<IActionResult> DisableClient(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Index");
            }

            var client = await _client.DisableClientAsync(id);
            
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

            TempData["ClientStatus"] = BusinessManager.ClientStatus;

            return RedirectToAction("Index");
        }
    }
}
