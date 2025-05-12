using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Admin.Controllers
{
    public class SettingsController : Controller
    {
        [Authorize(Policy = "Settings:view")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
