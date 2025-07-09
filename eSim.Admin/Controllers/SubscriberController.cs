
using eSim.Common.Enums;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Subscribers;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eSim.Admin.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly ISubscriberService _subscriber;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClient _client;
        public SubscriberController(ISubscriberService subscriber, UserManager<ApplicationUser> userManager, IClient client)
        {
            _subscriber = subscriber;
            _userManager = userManager;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string ClientId)
        {
            IQueryable<SubscribersResponseViewModel> subscriberList = null!;

            if (ClientId == null)
            {
                subscriberList = await _subscriber.GetClient_SubscribersListAsync(null);
            }
            else
            {
                subscriberList = await _subscriber.GetClient_SubscribersListAsync(ClientId);
            }
                return View(subscriberList.ToList());
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

