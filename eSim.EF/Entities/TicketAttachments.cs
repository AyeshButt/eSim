using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    [Table("TicketActivities", Schema = "client")]
    public class TicketActivities
    {
        [Key]
        public Guid Id { get; set; }
        public string TicketId { get; set; } = null!;
        [MaxLength(200)]
        public string Comment { get; set; } = null!;
        public int CommentType { get; set; }
        public bool IsVisibleToCustomer { get; set; }
        public string ActivityBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

    }
}
