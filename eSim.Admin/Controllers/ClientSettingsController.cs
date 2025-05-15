using eSim.Common.StaticClasses;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.Interfaces.Admin.Client;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eSim.Admin.Controllers
{
    public class ClientSettingsController : Controller
    {
        private readonly IClientSettings _clientSettings;

        public ClientSettingsController(IClientSettings clientSettings)
        {
            _clientSettings = clientSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Index", "Client");
            }

            var clientSettingsList = await _clientSettings.GetClientSettingsAsync(id);

            if (!clientSettingsList.Success)
            {
                ViewBag.ClientSettingsNotFound = BusinessManager.ClientSettingsNotFound;
            }

            TempData["clientid"] = id;
            TempData.Keep();

            return View(clientSettingsList.Data);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateClientSettings(Guid id)
        {
            var client = await _clientSettings.GetClientSettingsAsync(id);

            if (!client.Success)
            {
                client.Data = new ClientSettingsDTO()
                {
                    Id = Guid.NewGuid(),
                    ClientId = id,
                };
                return View(client.Data);
            }

            return View(client.Data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateClientSettings(ClientSettingsDTO input)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("UpdateClientSettings", new { id = input.Id });
            }

            input.CreatedBy = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            input.ModifiedBy = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            var client = await _clientSettings.UpdateClientSettingsAsync(input);

            if (!client.Success && client.Data is not null)
            {
                TempData["ClientError"] = client.Data;

                return RedirectToAction("UpdateClientSettings", new { id = input.Id });
            }

            if (!client.Success)
            {
                TempData["ClientNotFound"] = BusinessManager.ClientNotFound;

                return RedirectToAction("UpdateClientSettings", new { id = input.Id });
            }

            TempData["ClientSettingsUpdated"] = BusinessManager.ClientUpdated;
            
            return RedirectToAction("Index",new {id = input.ClientId});

        }

    }
}
