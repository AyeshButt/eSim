﻿using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Global;
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

        public async Task<Result<TicketDTO>> GetTicketDetailAsync(string trn)
        {
            var result = new Result<TicketDTO>();

            try
            {
                var ticket = await _db.Ticket.FirstOrDefaultAsync(t => t.TRN == trn);

                if (ticket == null)
                {
                    result.Success = false;
                    result.Message = "Ticket not found.";
                    return result;
                }

                var activities = await _db.TicketActivities
                    .Where(a => a.TicketId == ticket.Id.ToString())
                    .OrderByDescending(a => a.ActivityAt)
                    .ToListAsync();

       
                var subscriberIds = activities
                    .Where(a => Guid.TryParse(a.ActivityBy, out _))
                    .Select(a => Guid.Parse(a.ActivityBy))
                    .Distinct()
                    .ToList();

  
                var subscriberDict = await _db.Subscribers
                    .Where(s => subscriberIds.Contains(s.Id))
                    .ToDictionaryAsync(s => s.Id, s => s.FirstName + " " + s.LastName);



        
                var comments = activities.Select(a => new TicketActivityDTO
                {
                    Comment = a.Comment,
                    ActivityAt = a.ActivityAt,
                    ActivityBy = Guid.TryParse(a.ActivityBy, out var guid) && subscriberDict.ContainsKey(guid)
                        ? subscriberDict[guid]   
                        : a.ActivityBy,
               
                }).ToList();


                var attachments = await _db.TicketAttachments
                    .Where(att => att.TicketId == ticket.Id.ToString())
                    .Select(att => att.Attachment)
                    .ToListAsync();

           
                result.Data = new TicketDTO
                {
                    Id = ticket.Id,
                    TRN = ticket.TRN,
                    Subject = ticket.Subject,
                    Description = ticket.Description,
                    TicketType = ticket.TicketType,
                    CreatedAt = ticket.CreatedAt,
                    Status = ticket.Status,
                    Comments = comments,
                    Attachments = attachments
                };

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Something went wrong: " + ex.Message;
            }

            return result;
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

        public async Task SaveTicketCommentAsync(TicketCommentRequest request, string activityBy)
        {
            var ticket = await _db.Ticket.FirstOrDefaultAsync(t => t.TRN == request.TRN);
            if (ticket == null) throw new Exception("Ticket not found.");

            var activity = new TicketActivities
            {
                TicketId = ticket.Id.ToString(),
                Comment = request.Comment,
                CommentType = request.CommentType,
                IsVisibleToCustomer = request.IsVisibleToCustomer,
                ActivityBy = activityBy,
                ActivityAt = BusinessManager.GetDateTimeNow(),
                
            };

            await _db.TicketActivities.AddAsync(activity);
            await _db.SaveChangesAsync();
        }
    }
}
