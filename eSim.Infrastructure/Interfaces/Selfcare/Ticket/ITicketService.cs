using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Selfcare.Ticket;
using eSim.Infrastructure.DTOs.Ticket;

namespace eSim.Infrastructure.Interfaces.Selfcare.Ticket
{
    public interface ITicketService
    {
        public Task<Result<List<TicketsResponseDTO>>> Get();

        public Task<Result<List<TicketTypeResponseDTO>>> GetTicketType();
        public Task<Result<string>> Create(TicketRequestViewModel dto);
    }
}
