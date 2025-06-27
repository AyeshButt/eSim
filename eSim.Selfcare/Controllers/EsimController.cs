using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Raven.Client.Linq.LinqPathProvider;

namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class EsimController(IEsimService esim) : Controller
    {
        private readonly IEsimService _esim = esim;

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await _esim.GetEsimListAsync();

            ViewBag.Message = TempData["BundleAppliedSuccessfully"] ?? null;

            return View(response.Data);
        }

        [HttpGet]
        public IActionResult Details(string iccid)
        {
            return View(model: iccid);
        }

        [HttpGet]
        public async Task<IActionResult> EsimDetails(string iccid)
        {
            var response = await _esim.GetEsimDetailsAsync(iccid);

            return PartialView("_EsimDetails", response.Data);
        }

        public async Task<IActionResult> EsimHistory(string iccid)
        {
            var response = await _esim.GetEsimHistoryAsync(iccid);

            return PartialView("_EsimHistory", response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriberInventory(string iccid)
        {
            var response = await _esim.GetSubscriberInventoryAsync();

            if (!response.Success)
                return BadRequest(response);

            SubscriberInventoryViewModel model = new SubscriberInventoryViewModel()
            {
                Iccid = iccid,
                Inventory = response?.Data?.Select(p => new SelectListItem
                {
                    Value = p.Item,
                    Text = p.Description
                }).ToList() ?? new List<SelectListItem>()
            };

            return PartialView("_BundleSelectionPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyBundleToExistingEsim(SubscriberInventoryViewModel model)
        {
            var request = new ApplyBundleToExistingEsimRequest()
            {
                Iccid = model.Iccid,
                Name = model.Bundle
            };

            request.Name = "3243";

            var response = await _esim.ApplyBundleToExistingEsimAsync(request);
            
            //not yet completed
            if (!response.Success)
                return BadRequest(response);

            TempData["BundleAppliedSuccessfully"] = BusinessManager.BundleAppliedSuccessfully;

            return RedirectToAction("List");
        }

    }

}
