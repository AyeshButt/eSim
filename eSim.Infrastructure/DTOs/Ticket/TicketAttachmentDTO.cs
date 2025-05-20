using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketAttachmentDTO
    {
        
        [Required(ErrorMessage = "TRN is required.")]
        public string TRN { get; set; }

        public int AttachmentType { get; set; } 
    
        public Guid? ActivityId { get; set; }
        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; } = null!;
    }
}
