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
        public async Task<IActionResult> Index(string id)
        {
            if(!Guid.TryParse(id,out Guid parseId))
            {
                return RedirectToAction("Error", "Account");
            }
            
            var verifyClient = await _clientSettings.CheckIfClientExists(parseId);

            if (!verifyClient.Success)
            {
                return RedirectToAction("Error", "Account");
            }
            
            var clientSettings = await _clientSettings.GetClientSettingsAsync(parseId);

            ViewBag.clientid = id;

            return View(clientSettings.Data);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateClientSettings(string id)
        {
            if (!Guid.TryParse(id, out Guid parseId))
            {
                return RedirectToAction("Error", "Account");
            }
            var verifyClient = await _clientSettings.CheckIfClientExists(parseId);

            if (!verifyClient.Success)
            {
                return RedirectToAction("Error", "Account");
            }

            var clientsettings = await _clientSettings.GetClientSettingsAsync(parseId);

            if (!clientsettings.Success)
            {
                clientsettings.Data = new ClientSettingsDTO()
                {
                    ClientId = parseId,
                    Id = Guid.NewGuid(),
                };
            }

            return View(clientsettings.Data);
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

            if (!client.Success)
            {
                TempData["ClientError"] = client.Message;

                return RedirectToAction("UpdateClientSettings", new { id = input.Id });
            }

            TempData["ClientSettingsUpdated"] = BusinessManager.ClientSettingsUpdated;
            
            return RedirectToAction("Index",new {id = input.ClientId});

        }

    }
}
