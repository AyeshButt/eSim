using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketStatusDTO
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
    }
}
