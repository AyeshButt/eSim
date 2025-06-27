using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class SubscriberController : Controller
    {
        public IActionResult Subscriber()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }

    }
}
