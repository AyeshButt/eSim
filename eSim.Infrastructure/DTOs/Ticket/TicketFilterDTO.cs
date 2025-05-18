using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketFilterDTO
    {
        public int? Status { get; set; }
        public int? Type { get; set; }
        public string? TRN { get; set; }
        public string? Date { get; set; }
    }
}
