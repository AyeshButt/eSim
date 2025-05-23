using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ActionName("Analytics")]
        public IActionResult Analytics()
        {
            return View();
        }

        [ActionName("CRM")]
        public IActionResult CRM()
        {
            return View();
        }

        [ActionName("Crypto")]
        public IActionResult Crypto()
        {
            return View();
        }

        [ActionName("Projects")]
        public IActionResult Projects()
        {
            return View();
        }

        [ActionName("NFT")]
        public IActionResult NFT()
        {
            return View();
        }

        [ActionName("Job")]
        public IActionResult Job()
        {
            return View();
        }
        public IActionResult UserClaims()
        {
            return View();
        }
        public IActionResult SettingClaims()
        {
            return View();
        }
    }
}
