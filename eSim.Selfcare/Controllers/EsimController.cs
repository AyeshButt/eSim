using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            return PartialView("_EsimHistory",response.Data);
        }

    }
}

