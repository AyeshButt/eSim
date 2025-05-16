using eSim.Infrastructure.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.Admin.Ticket
{
    public interface ITicket
    {
        public Task<IQueryable<TicketDTO>> GetAllTicketsAsync();
        public Task<IQueryable<TicketDTO>> FilterTicketsAsync(TicketDTO input);

    }
}
