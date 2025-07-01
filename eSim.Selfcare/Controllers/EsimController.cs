using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Azure;
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

        #region Esim list

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await _esim.GetEsimListAsync();

            return View(response.Data);
        }

        #endregion

        #region Esim partial view tabs

        [HttpGet]
        public IActionResult Details(string iccid)
        {
            return View(model: iccid);
        }

        #endregion

        #region Loading esim partial views

        [HttpPost]
        public async Task<IActionResult> LoadEsimPartialViews(string tab, string iccid)
        {
            switch (tab?.ToLower())
            {
                case "detail":

                    var esimDetails = await _esim.GetEsimDetailsAsync(iccid);

                    return PartialView("_EsimDetails", esimDetails.Data);

                case "appliedBundle":

                    var esimAppliedBundles = await _esim.GetEsimHistoryAsync(iccid);

                    return PartialView("_EsimHistory", esimAppliedBundles.Data);

                default:

                    var esimHistory = await _esim.GetEsimHistoryAsync(iccid);

                    return PartialView("_EsimHistory", esimHistory.Data);

            }
        }

        #endregion

        #region Get subscriber inventory for the bundles dropdown

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

        #endregion

        #region Apply bundle to existing esim if possible

        [HttpPost]
        public async Task<IActionResult> ApplyBundleToExistingEsim(SubscriberInventoryViewModel model)
        {
            var request = new ApplyBundleToExistingEsimRequest()
            {
                Iccid = model.Iccid, /*"8932042000007856141"*/
                Name = model.Bundle /*"esim_1GB_7D_AU_U"*/
            };
            if (!ModelState.IsValid)
                return Json(new { showModal = false });

            var result = await _esim.ApplyBundleToExistingEsimAsync(request);

            if (!result.Success)
            {
                return Json(new { showModal = true, url = Url.Action("IncompatibleBundleModal") });
            }

            TempData["BundleAppliedSuccessfully"] = BusinessManager.BundleAppliedSuccessfully;

            return Json(new { redirectUrl = Url.Action("List") });
        }

        #endregion

        #region Partial view for incompatible bundle

        [HttpGet]
        public IActionResult IncompatibleBundleModal(string bundleName)
        {
            return PartialView("_ApplyBundleToEsim", new IncompatibleBundleToNewEsimViewModel() { BundleName = bundleName });
        }

        #endregion

        #region Apply bundle to new esim where required

        [HttpPost]
        public async Task<IActionResult> ApplyBundleToEsim(IncompatibleBundleToNewEsimViewModel model)
        {
            var request = new ApplyBundleToEsimRequest
            {
                Name = model.BundleName,
            };

            var result = await _esim.ApplyBundleToEsimAsync(request);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                // Still redirect, so toast shows after reload
                return Json(new { redirectUrl = Url.Action("List") });
            }

            TempData["SuccessMessage"] = result.Message;

            return Json(new { redirectUrl = Url.Action("List") });
        }

        #endregion

        #region Download qr code

        [HttpGet]
        public async Task<IActionResult> DownloadEsimQR(string iccid)
        {
            var result = await _esim.DownloadEsimQRAsync(iccid);

            if (!result.Success || result.FileBytes == null)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("List");
            }

            return File(result.FileBytes, result.ContentType ?? "application/octet-stream", result.FileName ?? "download.png");
        }

    }

    #endregion
}


