using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Ticket;

namespace eSim.Infrastructure.Interfaces.Middleware.Ticket
{
    public interface ITicketServices
    {
        Task<TicketDTO> CreateTicketAsync(TicketDTO ticketDto);
    }
}
