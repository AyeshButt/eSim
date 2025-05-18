using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Middleware.Ticket;

namespace eSim.Implementations.Services.Middleware.Ticket
{
    public class TicketService : ITicketServices
    {
        public async  Task<TicketDTO> CreateTicketAsync(TicketDTO ticketDto)
        {
            if (ticketDto == null || string.IsNullOrWhiteSpace(ticketDto.Subject))
                return null;  


            await Task.Delay(10);
            
            ticketDto.Id = Guid.NewGuid();
            ticketDto.CreatedAt = DateTime.UtcNow;
           
            return ticketDto;
        }
    }
}
