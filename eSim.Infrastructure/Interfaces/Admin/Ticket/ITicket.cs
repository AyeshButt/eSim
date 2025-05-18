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
        public Task<IQueryable<TicketListDTO>> GetAllTicketsAsync();
        public Task<IQueryable<TicketStatusDTO>> GetStatusListAsync();
        public Task<IQueryable<TicketTypeDTO>> GetTypeListAsync();
 
    }
}
