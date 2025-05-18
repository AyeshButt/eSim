using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    [Table("TicketType", Schema = "ref")]
    public class TicketType
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
    }
}
