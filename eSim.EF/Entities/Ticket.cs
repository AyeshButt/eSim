using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    [Table("Ticket", Schema = "client")]

    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(15)]
        public string TRN { get; set; } = null!;
        [MaxLength(250)]
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int TicketType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
    }
}
