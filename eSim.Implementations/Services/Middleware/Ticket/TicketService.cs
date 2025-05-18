using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Result<string?>> CreateTicketAsync(TicketRequestDTO ticketDto)
        {

            try
            {

                if (ticketDto.Subject.Contains("err"))
                {
                    throw new Exception("hello");
                }

                var ticket = new eSim.EF.Entities.Ticket
                {
                    Id = Guid.NewGuid(),
                    TRN = BusinessManager.GenerateTRN(),  // Now this works
                    Subject = ticketDto.Subject,
                    Description = ticketDto.Description,
                    TicketType = ticketDto.TicketType,
                    Status = 0,
                    CreatedAt = BusinessManager.GetDateTimeNow()
                };
                _Db.Ticket.Add(ticket);
                await _Db.SaveChangesAsync();


                return new Result<string?>
                {
                    Data = ticket.TRN
                };
            }
            catch (Exception ex)
            {
                return new Result<string?>
                {
                    Success = false,
                    Message = "Error occurred while posting the ticket.",
                };
            }
        }

        public Result<List<TicketTypeResponseDTO>> GetTicketType()
        {
            List<TicketTypeResponseDTO> list = _Db.TicketType.AsNoTracking().
                Select(a => new TicketTypeResponseDTO() { Id = a.Id, Value = a.Type }).ToList();


            return new Result<List<TicketTypeResponseDTO>>() { Data = list };
        }

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

            return new Result<IQueryable<TicketsResponseDTO>>() { Data = tickets.OrderByDescending(a=>a.CreatedAt) };
        }
    }
}
