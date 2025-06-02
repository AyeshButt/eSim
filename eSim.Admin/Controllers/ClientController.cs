using eSim.Common.StaticClasses;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Admin.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace eSim.Admin.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClient _client;
        private readonly IEmailService _email;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(IClient client, IEmailService email, UserManager<ApplicationUser> userManager)
        {
            _client = client;
            _email = email;
            _userManager = userManager;
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
            string token = string.Empty;
            string encodedToken = string.Empty;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("CreateClient");
            }

            input.CreatedBy = input.ModifiedBy = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            var client = await _client.CreateClientAsync(input);

            if (!client.Success)
            {
                TempData["ClientError"] = client.Message;

                return RedirectToAction("CreateClient");

            }

            TempData["ClientCreated"] = BusinessManager.ClientCreated;

            if (client.Data is null)
            {
                TempData["EmailNotSent"] = BusinessManager.EmailNotSent;

                return RedirectToAction("Index");
            }

            //changes

            var findUser = await _userManager.FindByIdAsync(client.Data.UserId);
            
            if (findUser is null)
            {
                TempData["EmailNotSent"] = BusinessManager.EmailNotSent;
                return RedirectToAction("Index");

            }

            token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
            encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            client.Data.Token = encodedToken;

            #region Sending verification and password email to the user after client creation

            var confirmationEmail = _email.SendConfirmationEmail(input.PrimaryEmail, client.Data);

            var passwordEmail = _email.SendPasswordEmail(input.PrimaryEmail, client.Data);

            TempData[confirmationEmail && passwordEmail ? "EmailReceived" : "EmailNotReceived"] = confirmationEmail && passwordEmail ? BusinessManager.EmailReceived : BusinessManager.EmailNotReceived;

            #endregion

            return RedirectToAction("Index");
        }


        [Authorize(Policy = "Clients:edit")]

        [HttpGet]
        public async Task<IActionResult> UpdateClient(string id)
        {
            if (!Guid.TryParse(id, out Guid parseId))
            {
                return RedirectToAction("Error", "Account");
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
                return RedirectToAction("Error", "Account");
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

        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string PrimaryEmail, string Id)
        {
            Guid.TryParse(Id, out Guid parsedId);

            bool isUnique = await _client.IsEmailUniqueAsync(PrimaryEmail, parsedId);

            return Json(isUnique);
        }
        [HttpGet]
        public async Task<IActionResult> IsNameAvailable(string Name, string Id)
        {
            Guid.TryParse(Id, out Guid parsedId);

            bool isUnique = await _client.IsNameUniqueAsync(Name, parsedId);

            return Json(isUnique);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string userId, string token)
        {
            string decodedToken;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) ||
                await _userManager.FindByIdAsync(userId) is not ApplicationUser user)
            {
                return RedirectToAction("Error", "Account");
            }

            if (user.EmailConfirmed)
            {
                TempData["EmailAlreadyConfirmed"] = BusinessManager.AlreadyVerified;

                return View();
            }

            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                return RedirectToAction("Error", "Account");
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                return View();
            }

            TempData["VerificationFailed"] = BusinessManager.VerificationFailed;
            return View();
        }
    }
}
