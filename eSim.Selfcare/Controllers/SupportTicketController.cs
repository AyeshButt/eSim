using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Selfcare.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class SupportTicketController : Controller
    {
        private readonly ITicketService _ts;

        public SupportTicketController(ITicketService ts)
        {
            _ts = ts;
        }

        #region Get Ticket
        [HttpGet]
        public async Task<IActionResult> List(string? search, string? dateRange, string? status)
        {
            var data = await _ts.Get();

            //search on the base of TRN
            if (!string.IsNullOrWhiteSpace(search))
            {
                data.Data = data.Data.Where(x => x.TRN.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                return View(data);
            }

            //search on the bases of status
            //if (!string.IsNullOrWhiteSpace(status) && status != "all")
            //{
            //    data.Data = data.Data.Where(x=> x.)
            //}
            return View(data);
        }

        #endregion

        [HttpGet]
        public IActionResult TicketDetails()
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
                    TempData["ToastMessage"] = resp.Message + " " + "Ticket Has been genrated";
                    TempData["ToastType"] = "Success";
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.ToastMessage = resp.Message;
                    ViewBag.ToastType = "error";
                    return View();
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
