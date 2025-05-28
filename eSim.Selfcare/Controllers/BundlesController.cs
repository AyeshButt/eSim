using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class BundlesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StandardEsimBundles()
        {
            return View();
        }
    }
}
