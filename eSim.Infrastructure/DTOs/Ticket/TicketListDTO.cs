using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketListDTO : TicketDTO
    {
        public string StatusName { get; set; }  = string.Empty;
        public string TypeName { get; set; } = string.Empty;
    }
}
