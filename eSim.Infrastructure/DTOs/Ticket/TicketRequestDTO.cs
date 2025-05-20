using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Ticket
{
    public class TicketRequestDTO
    {
        [Required (ErrorMessage ="select ticket Type")]
        public int TicketType { get; set; }

        [Required(ErrorMessage ="Subject is required")]
        [MaxLength(250)]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; } = null!;
       
    }


    public class TicketsResponseDTO
    {
        public string TRN { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }



}
