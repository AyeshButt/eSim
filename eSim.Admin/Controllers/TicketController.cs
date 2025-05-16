using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Admin.Ticket;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            var ticketList = await _ticket.GetAllTicketsAsync();

            return View(ticketList.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Filter(TicketDTO input)
        {
            return View();

        }
    }
}
