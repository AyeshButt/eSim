using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Admin.Ticket
{
    public class TicketResponseViewModel
    {
        public string TRN { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
