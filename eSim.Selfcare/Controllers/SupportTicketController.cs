using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Selfcare.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class SupportTicketController : Controller
    {
        private readonly ITicketService _ts;

        public SupportTicketController(ITicketService ts)
        {
            _ts = ts;
        }

        #region Get Ticket
        [HttpGet]
        public async Task<IActionResult> ListView()
        {
            var data = await _ts.Get();
            return View(data);
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> TicketDetails()
        {
            return View();
        }

        #region Open new Ticket
        [HttpGet]
        public async Task<IActionResult> OpenNewTicket()
        {
            var model = new TicketRequestDTO();
            var data = await _ts.GetTicketType();
            ViewBag.Types = data.Data;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> OpenNewTicket(TicketRequestDTO model) 
        {
            if (ModelState.IsValid)
            {
                var resp = await _ts.Create(model);

                if (resp.Success)
                {
                    return RedirectToAction("ListView");
                }
                else
                {
                    return View(resp);
                }
            }
            else
            {
                var data = new Result<TicketRequestDTO>() { Success = false, Message = "Check you input data" };
                return View(data);
            }
        }

        #endregion
    }
}
