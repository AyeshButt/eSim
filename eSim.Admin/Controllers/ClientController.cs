using eSim.Common.StaticClasses;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Admin.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eSim.Admin.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClient _client;
        private readonly IEmailService _email;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public ClientController(IClient client, IEmailService email, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _client = client;
            _email = email;
            _userManager = userManager;
            _config = config;
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

            input.CreatedBy = input.ModifiedBy = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            var client = await _client.CreateClientAsync(input);

            if (!client.Success)
            {
                TempData["ClientError"] = client.Message;
                return RedirectToAction("CreateClient");

            }

            TempData["ClientCreated"] = BusinessManager.ClientCreated;

            //client verification email

            var verificationSuccess = SendVerificationEmail(input.PrimaryEmail, type: "verification", input: client.Data);

            // client password email

            if (verificationSuccess)
            {
                var passwordSuccess = SendVerificationEmail(input.PrimaryEmail,input:client.Data);

                if (passwordSuccess)
                {
                    TempData["PasswordEmailReceieved"] = BusinessManager.PasswordEmailReceieved;
                }
            }
            else
            {
                TempData["EmailNotReceived"] = BusinessManager.EmailNotReceived;
            }

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

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                TempData["InvalidUser"] = BusinessManager.InvalidUser;

                return View();
            }

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    TempData["VerificationFailed"] = BusinessManager.VerificationFailed;

                    return View();
                }
            }
            else
            {
                TempData["AlreadyVerified"] = BusinessManager.AlreadyVerified;

            }

            return View();
        }
        private bool SendVerificationEmail(string primaryEmail, string type = "", ClientUserDTO? input = null)
        {
            var baseUrl = _config.GetValue<string>("VerificationEmail:url") ?? string.Empty;

            EmailDTO email = new EmailDTO()
            {
                To = primaryEmail
            };

             
            if (input != null && !string.IsNullOrEmpty(baseUrl))
            {
                if (type == "verification")
                {
                    //verification configuration

                    email.Subject = BusinessManager.Verification_EmailSubject;
                    email.Body = BusinessManager.Verification_EmailBody(input.UserId, baseUrl);

                }
                else
                {
                    //password configuration

                    email.Subject = BusinessManager.Password_EmailSubject;
                    email.Body = input.Password;

                }

                var result = _email.SendEmail(email);
                return result.Success;
            }

            return false;
        }
    }
}
