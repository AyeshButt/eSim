using System;
using System.Threading.Tasks;
using eSim.Infrastructure.Interfaces.Admin.Esim;
using eSim.Infrastructure.Interfaces.Admin.Order;
using eSim.Infrastructure.Interfaces.Middleware.Esim;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Admin.Controllers
{
    public class SimController : Controller
    {
        private readonly IEsims _esim;

        public SimController( IEsims esim)
        {
            _esim = esim;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var esimsList = await _esim.GetEsimListForAllSubscribersAsync();
            return View(esimsList);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {

            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TabPartialViews(string tab, string iccid) 
        {
            switch (tab)
            {
                case "Detail":
                    var esimdetail = await _esim.GetEsimDetailsAsync(iccid);
                    return PartialView("_EsimDetailsPartialView", esimdetail.Data);

                case "AppliedBundles":
                    var appliedBundles = await _esim.GetListBundlesAppliedToEsimAsync(iccid);
                    return PartialView("_EsimAppliedBundlesPartailView", appliedBundles.Data);

                default:
                    var esimHistory = await _esim.GetEsimHistoryAsync(iccid);
                    return PartialView("_EsimHistoryPartialView", esimHistory.Data);
            }

            
        }

    }
}
