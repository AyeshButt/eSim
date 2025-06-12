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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace eSim.Implementations.Services.Middleware.Ticket
{
    public class TicketService : ITicketServices
    {
        private readonly ApplicationDbContext _Db;
        public TicketService(ApplicationDbContext Db)
        {
            _Db = Db;
        }

        public async Task<Result<TicketCommentDTORequest>> AddCommentAsync(TicketCommentDTORequest input, string userId)
        {
            var result = new Result<TicketCommentDTORequest>();

            try
            {

                var ticket = await _Db.Ticket.FirstOrDefaultAsync(u => u.TRN == input.TRN);

                if (ticket == null)
                {
                    result.Success = false;
                    result.Message = string.Empty;

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

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;

            }
            return result;
        }



        #region CreateTicket
        public async Task<Result<string?>> CreateTicketAsync(TicketRequestDTORequest ticketDto)
        {
            var result = new Result<string?>();
            try
            {


                var ticket = new eSim.EF.Entities.Ticket
                {
                    Id = Guid.NewGuid(),
                    TRN = BusinessManager.GenerateTRN(),
                    Subject = ticketDto.Subject,
                    Description = ticketDto.Description,
                    TicketType = ticketDto.TicketType,
                    Status = 0,
                    CreatedAt = BusinessManager.GetDateTimeNow()
                };

                _Db.Ticket.Add(ticket);
                await _Db.SaveChangesAsync();

                result.Success = true;
                result.Message = BusinessManager.TicketCreated;
                result.Data = ticket.TRN;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion
        #region TicketDetail
        public async Task<Result<TicketDTO?>> GetTicketDetailAsync(string trn)
        {
            try
            {
                var ticket = await _Db.Ticket
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TRN == trn);

                if (ticket == null)
                    return new Result<TicketDTO?> { Success = false, Message = "Ticket not found." };

                var ticketType = await _Db.TicketType
                    .AsNoTracking()
                    .Where(t => t.Id == ticket.TicketType)
                    .Select(t => t.Type)
                    .FirstOrDefaultAsync();

                var attachments = await _Db.TicketAttachments
                    .Where(a => a.TicketId == ticket.Id.ToString())
                    .Select(a => a.Attachment)
                    .ToListAsync();

                var detail = new TicketDTO
                {
                    TRN = ticket.TRN,
                    Subject = ticket.Subject,
                    Description = ticket.Description,
                    TicketType = ticket.TicketType,
                    Status = ticket.Status,
                    CreatedAt = ticket.CreatedAt,
                    Attachments = attachments
                };

                return new Result<TicketDTO?>
                {
                    Success = true,
                    Message = "Ticket detail retrieved successfully.",
                    Data = detail
                };
            }
            catch (Exception)
            {
                return new Result<TicketDTO?>
                {
                    Success = false,
                    Message = "Error fetching ticket detail."
                };

            }

        }
        #endregion

        #region GetTicketType
        public Result<List<TicketTypeResponseDTO>> GetTicketType()
        {
            List<TicketTypeResponseDTO> list = _Db.TicketType.AsNoTracking().
                Select(a => new TicketTypeResponseDTO() { Id = a.Id, Value = a.Type }).ToList();


            return new Result<List<TicketTypeResponseDTO>>() { Data = list };
        }
        #endregion

        #region TicketsResponse
        public Result<IQueryable<TicketsResponseDTO>> Tickets()
        {
            var types = _Db.TicketType.AsNoTracking();

            var tickets = _Db.Ticket.Select(a => new TicketsResponseDTO
            {
                CreatedAt = a.CreatedAt,
                Subject = a.Subject,
                TRN = a.TRN,
                Type = types.FirstOrDefault(a => a.Id == a.Id).Type
            });

            return new Result<IQueryable<TicketsResponseDTO>>() { Data = tickets.OrderByDescending(a => a.CreatedAt) };
        }
        #endregion

        #region TicketAttachment
        public async Task<Result<string?>> UploadAttachmentAsync(TicketAttachmentDTORequest dto)
        {
            var result = new Result<string?>();
            try
            {
                // TicketTypeEnum.attachmentType.ToString();
                Guid? activityId = null;
                var ticket = await _Db.Ticket.FirstOrDefaultAsync(x => x.TRN == dto.TRN);
                if (ticket == null)

                { result.Success = false; result.Message = BusinessManager.Ticketnotfound; }
                ;


                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);




                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
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

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = ex.Message;

            }
            return result;
        }
        #endregion
    }
}
