using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
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
                return View(bundle);
            }
            return View(bundle);
        }


        [HttpGet]
        public async  Task<IActionResult> Detail(BundleNameDTO input)
        {
            var bundle = await _bundelService.BundleDetail(input);

            Console.WriteLine("Bundle Detail" + bundle.Data);

            return PartialView("_BundleDetailPartial", bundle.Data);
        }
        public IActionResult EsimBundles()
        {
            return View();
        }
        public IActionResult FixedBundles()
        {
            return View();
        }
        public IActionResult LongDurationBundles()
        {
            return View();
        }
        public IActionResult UnlimitedEssentialBundles()
        {
            return View();
        }
        public IActionResult UnlimitedLiteBundles()
        {
            return View();
        }
        public IActionResult UnlimitedPlusBundles()
        {
            return View();
        }
        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}
