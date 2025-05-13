using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Admin.Controllers
{
    public class PoolController : Controller
    {
        [Authorize(Policy = "Pool:view")]

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Pool:create")]
        public IActionResult Create()
        {
            return View();
        }


    }
}
