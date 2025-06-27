using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Ticket;
using Microsoft.AspNetCore.Http;

namespace eSim.Infrastructure.DTOs.Selfcare.Ticket
{
    public class TicketRequestViewModel 
    {
        public List<TicketTypeResponse> Types { get; set; } = new();

        [Required(ErrorMessage = "select ticket Type")]
        public int TicketType { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [MaxLength(250)]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; } = null!;
    }
}
