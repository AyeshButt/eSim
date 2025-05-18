using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    [Table("TicketStatus", Schema = "ref")]
    public class TicketStatus
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
    }
}
