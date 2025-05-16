using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Admin.Ticket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Implementations.Services.Ticket
{
    public class TicketService : ITicket
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TicketService> _logger;
        public TicketService(ApplicationDbContext db, ILogger<TicketService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IQueryable<TicketDTO>> FilterTicketsAsync(TicketDTO input)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<TicketDTO>> GetAllTicketsAsync()
        {
            var ticketList = _db.Ticket.AsNoTracking().Select(u => new TicketDTO()
            {
                Id = u.Id,
                TRN = u.TRN,
                TicketType = u.TicketType,
                Subject = u.Subject,
                Status = u.Status,
                Description = u.Description,
                CreatedAt = u.CreatedAt,
            }).AsQueryable();

            return await Task.FromResult(ticketList);
        }
    }
}
