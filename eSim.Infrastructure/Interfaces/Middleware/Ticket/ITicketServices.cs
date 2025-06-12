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
        Task<Result<string?>> CreateTicketAsync(TicketRequestDTORequest ticketDto);
        Result<IQueryable<TicketsResponseDTO>> Tickets();
        Task<Result<string?>> UploadAttachmentAsync(TicketAttachmentDTORequest dto);

        Task<Result<TicketDTO?>> GetTicketDetailAsync(string trn);
        Result<List<TicketTypeResponseDTO>> GetTicketType();

        Task<Result<TicketCommentDTORequest>> AddCommentAsync(TicketCommentDTORequest dto,string userId);

    }
}
