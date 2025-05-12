using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Admin.Controllers
{
    public class ActiveBundleController : Controller
    {
        [Authorize(Policy = "Active Bundles:view")]

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Active Bundles:generate")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
