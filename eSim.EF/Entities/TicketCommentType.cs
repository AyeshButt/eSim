using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    [Table("TicketCommentType", Schema = "ref")]
    public class TicketCommentType
    {
        public int Id { get; set; }
        public string CommentType { get; set; } = null!;

    }
}
