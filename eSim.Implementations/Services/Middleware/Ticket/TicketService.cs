using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.Enums;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Middleware.Ticket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Raven.Client.Util;
using static Raven.Database.Indexing.IndexingWorkStats;


namespace eSim.Implementations.Services.Middleware.Ticket
{
    public class TicketService : ITicketServices
    {
        private readonly ApplicationDbContext _Db;
        public TicketService(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        #region AddComment
        public async Task<Result<TicketCommentRequest>> AddCommentAsync(TicketCommentRequest input, string userId)
        {
            var result = new Result<TicketCommentRequest>();

            try
            {

                var ticket = await _Db.Ticket.FirstOrDefaultAsync(u => u.TRN == input.TRN);

                if (ticket == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Ticketnotfound;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }

                var comment = new TicketActivities
                {
                    Id = Guid.NewGuid(),
                    TicketId = ticket.Id.ToString(),
                    Comment = input.Comment,
                    CommentType = (int)CommentType.customer,
                    IsVisibleToCustomer = input.IsVisibleToCustomer,
                    ActivityBy = userId,
                    ActivityAt = BusinessManager.GetDateTimeNow()
                };

                await _Db.TicketActivities.AddAsync(comment);
                await _Db.SaveChangesAsync();

                result.Message = BusinessManager.Commentadded;
                result.StatusCode = StatusCodes.Status200OK;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }
            return result;
        }

        #endregion

        #region CreateTicket
        public async Task<Result<string?>> CreateTicketAsync(TicketRequest input, Guid loggeruser)
        {
            var result = new Result<string?>();


            try
            {
                if (input.TicketType == 0 ||!await _Db.TicketType.AnyAsync(t => t.Id == input.TicketType))
                {
                    result.Success = false;
                    result.Message = BusinessManager.InvalidTicketType;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }


                var ticket = new eSim.EF.Entities.Ticket
                {
                    Id = Guid.NewGuid(),
                    TRN = BusinessManager.GenerateTRN(),
                    Subject = input.Subject,
                    Description = input.Description,
                    TicketType = input.TicketType,
                    CreatedBy = loggeruser.ToString(),
                    Status = (int)Common.Enums.TicketStatus.Open,
                    ModifiedAt=BusinessManager.GetDateTimeNow(),
                    CreatedAt = BusinessManager.GetDateTimeNow()
                };
            
                _Db.Ticket.Add(ticket);
                await _Db.SaveChangesAsync();

                result.Success = true;
                result.Message = BusinessManager.TicketCreated;
                result.Data = ticket.TRN;
                result.StatusCode = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return result;
        }

        #endregion
        #region TicketDetail
        public async Task<Result<TicketDTO?>> GetTicketDetailAsync(string trn)
        {
            var result = new Result<TicketDTO?>();
            try
            {
                var ticket = await _Db.Ticket
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TRN == trn);

                if (ticket == null)
               { result.Success = false; 
                    result.Message = BusinessManager.Ticketnotfound; 
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;                }
                ;

                var ticketData = await (from t in _Db.Ticket
                                        join tt in _Db.TicketType on t.TicketType equals tt.Id
                                        where t.TRN == trn
                                        select new
                                        {
                                            Ticket = t,
                                            TicketType = tt.Type,

                                            Attachments = _Db.TicketAttachments
                                                .Where(a => a.TicketId == t.Id.ToString())
                                                .Select(a => a.Attachment)
                                                .ToList(),

                                            Activities = (from activity in _Db.TicketActivities
                                                          join user in _Db.Subscribers
                                                          on activity.ActivityBy equals user.Id.ToString()
                                                          where activity.TicketId == t.Id.ToString()
                                                          orderby activity.ActivityAt descending
                                                          select new TicketActivityDTO
                                                          {
                                                              Comment = activity.Comment,
                                                              ActivityBy = user.Email,
                                                              ActivityAt = activity.ActivityAt
                                                          }).ToList()
                                        }).FirstOrDefaultAsync();


                var detail = new TicketDTO
                {  Id = ticket.Id,
                    TRN = ticketData.Ticket.TRN,
                    Subject = ticketData.Ticket.Subject,
                    Description = ticketData.Ticket.Description,
                    TicketType = ticketData.Ticket.TicketType,
                    Status = ticketData.Ticket.Status,
                    CreatedAt = ticketData.Ticket.CreatedAt,
                    Attachments = ticketData.Attachments,
                    Comments = ticketData.Activities
                };



                {
                    result.Success = true;
                    result.Message = BusinessManager.TicketdetailRetrieved;
                    result.StatusCode = StatusCodes.Status200OK;
                        result.Data = detail;
                   
                };
            }
            catch (Exception ex)
            {
                
                {
                    result.Success = false;
                    result.Message = ex.Message;
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    return result;
                };

          
            }
            return result;
        }
        #endregion

        #region GetTicketType   
        public Result<List<TicketTypeResponse>> GetTicketType()
        {
            List<TicketTypeResponse> list = _Db.TicketType.AsNoTracking().
                Select(a => new TicketTypeResponse() { Id = a.Id, Value = a.Type }).ToList();


            return new Result<List<TicketTypeResponse>>() { Data = list ,StatusCode=StatusCodes.Status200OK};
        }
        #endregion

        #region TicketsResponse
        public Result<IQueryable<TicketsResponse>> Tickets(Guid loggeruser)
        {
            var userTickets = _Db.Ticket
       .AsNoTracking()
       .Where(a => a.CreatedBy == loggeruser.ToString())
       .ToList();

        
            var usedTypeIds = userTickets.Select(t => t.TicketType).Distinct().ToList();
            var usedStatusIds = userTickets.Select(t => t.Status).Distinct().ToList();

            // ✅ Get only relevant types and statuses
            var types = _Db.TicketType
                .AsNoTracking()
                .Where(t => usedTypeIds.Contains(t.Id))
                .ToList();

            var status = _Db.TicketStatus
                .AsNoTracking()
                .Where(s => usedStatusIds.Contains(s.Id))
                .ToList();

            // ✅ Final projection
            var tickets = userTickets
                .Select(a => new TicketsResponse
                {
                    CreatedAt = a.CreatedAt,
                    Subject = a.Subject,
                    TRN = a.TRN,
                    Type = types.FirstOrDefault(t => t.Id == a.TicketType)?.Type,
                    status = status.FirstOrDefault(s => s.Id == a.Status)?.Status
                })
                .AsQueryable();


            return new Result<IQueryable<TicketsResponse>>() 
            
            { Data = tickets.OrderByDescending(a => a.CreatedAt) ,StatusCode=StatusCodes.Status200OK};
        }
        #endregion

        #region TicketAttachment
        public async Task<Result<string?>> UploadAttachmentAsync(TicketAttachmentRequest input)
        {
            var result = new Result<string?>();
            try
            {
                // TicketTypeEnum.attachmentType.ToString();
                Guid? activityId = null;
                var ticket = await _Db.Ticket.FirstOrDefaultAsync(x => x.TRN == input.TRN);
                if (ticket == null)

                { result.Success = false; result.Message = BusinessManager.Ticketnotfound; result.StatusCode = StatusCodes.Status400BadRequest; return result; }
         


                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{input.File.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);




                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await input.File.CopyToAsync(stream);
                }


                var attachment = new TicketAttachments
                {
                    Id = Guid.NewGuid(),
                    TicketId = ticket.Id.ToString(),
                    Attachment = $"/uploads/{fileName}",
                    AttachmentType = (int)TicketTypeEnum.attachmentType,
                    ActivityId = activityId
                };

                _Db.TicketAttachments.Add(attachment);
                await _Db.SaveChangesAsync();


                result.Success = true;
                result.Message = BusinessManager.Attachmentuploaded;
                result.StatusCode = StatusCodes.Status200OK;

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
                

            }
            return result;
        }
        #endregion
    }
}
