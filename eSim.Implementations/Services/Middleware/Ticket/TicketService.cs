using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Middleware.Ticket;
using Microsoft.EntityFrameworkCore;

namespace eSim.Implementations.Services.Middleware.Ticket
{
    public class TicketService : ITicketServices
    {
        private readonly ApplicationDbContext _Db;
        public TicketService (ApplicationDbContext Db)
        {
            _Db = Db;
        }
        private string GenerateTRN()
        {
            return $"TRN-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
        public async Task<CreateTicketApiDto> CreateTicketAsync(CreateTicketApiDto ticketDto)
        {
            var ticket = new eSim.EF.Entities.Ticket
            {
                Id = Guid.NewGuid(),
                TRN = GenerateTRN(),  // Now this works
                Subject = ticketDto.Subject,
                Description = ticketDto.Description,
                TicketType = ticketDto.TicketType,
                Status = 0,
                CreatedAt = BusinessManager.GetDateTimeNow()
            };

            _Db.Ticket.Add(ticket);
            await _Db.SaveChangesAsync();

            // Update ticketDto with extra data if needed here...

            return ticketDto;

        }

    }
}
