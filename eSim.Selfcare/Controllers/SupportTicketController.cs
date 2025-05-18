using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class SupportTicketController : Controller
    {
        
        public IActionResult ListView()
        {
            return View();
        }

        public IActionResult TicketDetails()
        {
            return View();
        }
        
        public IActionResult OpenNewTicket()
        {
            return View();
        }
    }
}
