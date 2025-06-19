using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string iccid)
        {
            var result = new EsimDetailsPartialViewModel();

            result.EsimDetails = await _esim.GetEsimDetailsAsync(iccid);
            result.EsimHistory = await _esim.GetEsimHistoryAsync(iccid);

            return PartialView("_EsimDetailPartial", result);
        }

    }
}

