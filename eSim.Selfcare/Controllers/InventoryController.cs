using System.Threading.Tasks;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.Selfcare.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class InventoryController(IInventoryService service) : Controller
    {
        private readonly IInventoryService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            var response = await _service.GetListAsync();

            Console.WriteLine("response" + response.Data);

            return View(response.Data);
        }

        [HttpGet]

        public async Task<IActionResult> Detail(string ID)
        {
            var response = await _service.DetailAsync(ID);
            return PartialView("_InventoryDetailPartialView", response);
        }
    }
}
