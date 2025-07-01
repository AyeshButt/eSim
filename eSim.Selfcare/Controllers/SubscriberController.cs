using eSim.Common.StaticClasses;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class SubscriberController : Controller
    { private readonly ISubscriberService _Subscriber;
        public SubscriberController(ISubscriberService Subscriber)
        {
            _Subscriber = Subscriber;
        }
        public IActionResult Subscriber()
        {
            return View();
        }
        public async  Task<IActionResult> Detail(Guid subscriberId)
        {
            var result = await _Subscriber.GetSubscriberDetailAsync(subscriberId);
            return View(result);
            //return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);

        }

    }
}
