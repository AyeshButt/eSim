using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class CreateTicketApiDto
    {

        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int TicketType { get; set; }
    }
}
