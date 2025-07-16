using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketActivityDTO
    {
        public string Comment { get; set; } = null!;
        public string ActivityBy { get; set; } = null!;
        public DateTime ActivityAt { get; set; }


    }
}
