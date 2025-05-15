using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    [Table("TicketAttachments", Schema = "client")]
    public class TicketAttachments
    {
        [Key]
        public Guid Id { get; set; }
        public string TicketId { get; set; } = null!;
        public string Attachment { get; set; } = null!;
        public int AttachmentType { get; set; }

    }
}
