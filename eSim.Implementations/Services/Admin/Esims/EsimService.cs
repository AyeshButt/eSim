using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Admin.Ticket;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Admin.Esim;
using Microsoft.EntityFrameworkCore;

namespace eSim.Implementations.Services.Admin.Esims
{
    public class EsimService(ApplicationDbContext db) : IEsims
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<List<EsimsDTO>> GetAllAsync(string Id)
        {
            var EsimList = await _db.Esims
                .Where(o => o.SubscriberId == Id)
                .Select(o => new EsimsDTO()
                {
                    Iccid = o.Iccid,
                    CustomerRef = o.CustomerRef,
                    ActionDate = o.ActionDate,
                    LastAction = o.LastAction,
                    Physical = o.Physical,
                    AssignedDate = o.AssignedDate,
                }).ToListAsync();

            return EsimList;
        }

        public async Task<List<TicketResponseViewModel>> GetAllTicketAsync(string id)
        {
            var TicketsList = await _db.Ticket
                 .Where(t => t.CreatedBy == id)
                 .Select(t => new TicketResponseViewModel
                 {
                     TRN = t.TRN,
                     Subject = t.Subject,
                     status = t.Status,
                     CreatedAt = t.CreatedAt,
                     CreatedBy = t.CreatedBy,
                     ModifiedBy = t.ModifiedBy,
                     ModifiedAt = t.ModifiedAt,

                 }).ToListAsync();

            return TicketsList;
        }
    }
}
