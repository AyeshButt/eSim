using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketViewModel : TicketFilterDTO
    {
        public List<TicketListDTO> AllTickets = new List<TicketListDTO>();
    }
}
