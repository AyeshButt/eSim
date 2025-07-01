using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;
using eSim.Infrastructure.Interfaces.Selfcare.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class InventoryController(IInventoryService service, IEsimService esimService) : Controller
    {
        private readonly IInventoryService _service = service;
        private readonly IEsimService _esimService = esimService;

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            var response = await _service.GetListAsync();

            Console.WriteLine("response" + response.Data);

            return View(response.Data);
        }

        [HttpGet]

        public async Task<IActionResult> Detail(string id)
        {
            var response = await _service.DetailAsync(id);

            //return View(response);
            return View( response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateNew(string id)
        {
            var request = await _service.GenrateAsync(id);
            //ViewBag.Iccid = "8932042000007892043";
            //ApplyBundleToEsimResponse request = new();
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadQr(string id)
        {
            var QrCodeRequest = await _service.GenrateQR(id);
            byte[] QrCode = QrCodeRequest;
            return File(QrCode, "image/png", $"qr_{id}.png");
        }
        
        [HttpGet]
        public async Task<IActionResult> ApplyToExisting(string name)
        {

            var eSIMList = await _esimService.GetEsimListAsync();
            ViewBag.BundleName = name;
            return PartialView("_ExistingEsimsPartialView", eSIMList.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyToExisting(string iccid, string bundleName)
        {
            ApplyBundleToExistingEsimRequest model = new ApplyBundleToExistingEsimRequest()
            {
                Iccid = iccid,
                Name = bundleName
            };
            var applyBundle = await _esimService.ApplyBundleToExistingEsimAsync(model);
            //return RedirectToAction("Dashboard", "Index");
            if (applyBundle.Success)
            {
                return Json(new { success = true, message = "Bundle applied successfully!" });
            }

            return Json(new { success = false, message = "Failed to apply bundle." });
        }



    }
}
