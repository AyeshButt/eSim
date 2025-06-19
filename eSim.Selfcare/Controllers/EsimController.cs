using System.Threading.Tasks;
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
            var response = await _esim.GetEsimList();

            return View(response.Data);
        }

        [HttpGet]
        public ActionResult Detail(string iccid)
        {
            var detail = _esim.GetDetail(iccid);
            return PartialView("_EsimDetailPartial", detail.Result.Data);
        }
    }
}
