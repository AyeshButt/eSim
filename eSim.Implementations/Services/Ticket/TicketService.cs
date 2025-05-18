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

        public async Task<IQueryable<TicketListDTO>> GetAllTicketsAsync()
        {
            var ticketList = (from ti in _db.Ticket
                              join ty in _db.TicketType on ti.TicketType equals ty.Id
                              join st in _db.TicketStatus on ti.Status equals st.Id
                              select new TicketListDTO()
                              {
                                  Id = ti.Id,
                                  TRN = ti.TRN,
                                  StatusName = st.Status,
                                  TypeName = ty.Type,
                                  Status = ti.Status,
                                  CreatedAt = ti.CreatedAt,
                                  Description = ti.Description,
                                  Subject = ti.Subject,
                                  TicketType = ti.TicketType,
                              }
                              ).AsQueryable();

            return await Task.FromResult(ticketList);
        }

        public async Task<IQueryable<TicketStatusDTO>> GetStatusListAsync()
        {
            var statusList = _db.TicketStatus.AsNoTracking().Select(u => new TicketStatusDTO()
            {
                Id = u.Id,
                Status = u.Status,
            }).AsQueryable();

            return await Task.FromResult(statusList);
        }

        public async Task<IQueryable<TicketTypeDTO>> GetTypeListAsync()
        {
            var typeList = _db.TicketType.AsNoTracking().Select(u => new TicketTypeDTO()
            {
                Id = u.Id,
                Type = u.Type,
            }).AsQueryable();

            return await Task.FromResult(typeList);
        }
    }
}
