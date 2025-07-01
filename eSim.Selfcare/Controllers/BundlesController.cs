using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.DTOs.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class BundlesController : Controller
    {
        private readonly IBundleService _bundle;

        public BundlesController( IBundleService bundelService)
        {
            _bundle = bundelService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StandardEsimBundles()
        {
            var bundle = await _bundle.GetBundles();

            if (bundle.Success) 
            { 
                return View(bundle);
            }
            return View(bundle);
        }

        [HttpGet]
        public IActionResult OrderModal(string name,decimal price)
        {
            var model = new OrderModalViewModel
            {
                BundleName = name,
                Price = price,
            };

            return PartialView("_OrderModalPartialView", model);
        }
        [HttpGet]
        public async  Task<IActionResult> Detail(string name)
        {
            var bundle = await _bundle.BundleDetail(name);

            return PartialView("_BundleDetailPartial", bundle.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModalViewModel model)
        {
            var response = await _bundle.CreateOrderAsync(model);

            if (response.Success)
            {
                TempData["Success"] = response?.Data?.StatusMessage ?? string.Empty;
            }
            else
            {
                TempData["Error"] = response?.Data?.StatusMessage ?? string.Empty;
            }
            return Json(new { redirectUrl = Url.Action("StandardEsimBundles", "Bundles") });
        }

        public IActionResult HajjPromoBundles()
        {
            return View();
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

