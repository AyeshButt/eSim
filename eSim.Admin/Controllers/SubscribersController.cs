using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Admin.Controllers
{
    public class SubscribersController : Controller
    {
        [Authorize(Policy = "Subscribers:view")]

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Subscribers:create")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
