using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Admin.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eSim.Admin.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicket _ticket;

        public TicketController(ITicket ticket)
        {
            _ticket = ticket;
        }

        [HttpGet]
        public async Task<IActionResult> Index(TicketViewModel filterSearch)
        {
            var result = new TicketViewModel();

            var query = await _ticket.GetAllTicketsAsync();

            BindTicketStatus();
            BindTicketType();

            if (filterSearch.Type is not null)
            {
                query = query.Where(u => u.TicketType == filterSearch.Type);
            }
            if(filterSearch.Status is not null)
            {
                query = query.Where(u => u.Status == filterSearch.Status);
            }
            if(filterSearch.TRN is not null)
            {
                query = query.Where(u=>u.TRN == filterSearch.TRN);
            }
            if(filterSearch.Date is not null)
            {
                var split= filterSearch.Date.Split("to");
                var from = Convert.ToDateTime(split[0]);
                var to = Convert.ToDateTime(split[1]);
                query = query.Where(u => u.CreatedAt >= from && u.CreatedAt <= to);
            }
            result.AllTickets = query.ToList();
            result.Status = filterSearch.Status;
            result.TRN = filterSearch.TRN;
            result.Type = filterSearch.Type;
            result.Date = filterSearch.Date;

            return View(result);

        }
        private async void BindTicketStatus()
        {
            var status = await _ticket.GetStatusListAsync();

            ViewBag.Status = new SelectList(status.ToList(), "Id", "Status");

        }
        private async void BindTicketType()
        {
            var type = await _ticket.GetTypeListAsync();
            
            ViewBag.TicketType = new SelectList(type.ToList(), "Id", "Type");
        }
    }
}
