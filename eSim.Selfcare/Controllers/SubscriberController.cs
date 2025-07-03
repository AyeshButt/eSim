using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.Interfaces.Middleware;
using eSim.Infrastructure.Interfaces.Selfcare.Subscriber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class SubscriberController : Controller
    { private readonly ISubscriber _Subscriber;
        public SubscriberController(ISubscriber Subscriber)
        {
            _Subscriber = Subscriber;
        }
        public IActionResult Subscriber()
        {
            return View();
        }
        public async  Task<IActionResult> Detail()
        {
            var result = await _Subscriber.SubscriberDetailAsync();
            return View(result.Data);


        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubscriber( UpdateSubscriberDTORequest request)
        {
            var result = await _Subscriber.UpdateSubscriberAsync(request);
            return RedirectToAction(nameof(Detail));

        }

   

        [HttpPost]
        public async Task<IActionResult> ChangePassword( ChangePasswordDTORequest request)
        {
            var result=await _Subscriber.ChangePasswordAsync(request);
            return RedirectToAction(nameof(Detail));
        }
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage()
        {
            var result = await _Subscriber.UploadProfileImage();
            return RedirectToAction(nameof(Detail));
        }


    }
}
