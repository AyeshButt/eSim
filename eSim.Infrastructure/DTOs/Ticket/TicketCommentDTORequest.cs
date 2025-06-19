using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketCommentRequest
    {
        [Required(ErrorMessage = "TRN is required.")]
        public string TRN { get; set; } = null!;

        [Required(ErrorMessage = "Comment is required.")]
        public string Comment { get; set; } = null!;

        [Required(ErrorMessage = "Comment type is required.")]
        public int CommentType { get; set; }

        public bool IsVisibleToCustomer { get; set; } = false;
    }
}
