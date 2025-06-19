using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eSim.Infrastructure.Interfaces.Middleware.Ticket
{
    public interface ITicketServices
    {
        Task<Result<string?>> CreateTicketAsync(TicketRequest input);
        Result<IQueryable<TicketsResponse>> Tickets();
        Task<Result<string?>> UploadAttachmentAsync(TicketAttachmentRequest input);

        Task<Result<TicketDTO?>> GetTicketDetailAsync(string trn);
        Result<List<TicketTypeResponse>> GetTicketType();

        Task<Result<TicketCommentRequest>> AddCommentAsync(TicketCommentRequest input,string userId);

    }
}
