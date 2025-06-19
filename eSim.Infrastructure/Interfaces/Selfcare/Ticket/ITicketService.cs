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
        public Task<Result<List<TicketsResponse>>> Get();
        public Task<Result<List<TicketTypeResponse>>> GetTicketType();
        public Task<Result<string>> Create(TicketRequestViewModel dto);

        public Task<Result<TicketDetailDTO>> Detail(string trn);
    }
}
