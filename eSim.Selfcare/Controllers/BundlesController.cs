using System.Threading.Tasks;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class BundlesController : Controller
    {
        private readonly IBundleService _bundelService;

        public BundlesController( IBundleService bundelService)
        {
            _bundelService = bundelService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StandardEsimBundles()
        {
            var bundle = await _bundelService.GetBundles();

            if (bundle.Success) 
            { 
                return View(bundle.Data);
            }
            return View(bundle);
        }

        //[HttpPost]

        //public async Task<IActionResult> StandardEsimBundles()
        //{

        //}
    }
}
