using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Ticket;

namespace eSim.Infrastructure.DTOs.Selfcare.Ticket
{
    public class TicketRequestViewModel 
    {
        public List<TicketTypeResponseDTO> Types { get; set; } = new();

        [Required(ErrorMessage = "select ticket Type")]
        public int TicketType { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [MaxLength(250)]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; } = null!;
    }
}
