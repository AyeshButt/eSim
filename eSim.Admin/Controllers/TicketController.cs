﻿using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Admin.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eSim.Admin.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicket _ticket;

        public TicketController(ITicket ticket)
        {
            _ticket = ticket;
        }

        [HttpGet]
        public async Task<IActionResult> Index(TicketViewModel filterSearch)
        {
            var result = new TicketViewModel();

            var query = await _ticket.GetAllTicketsAsync();

            BindTicketStatus();
            BindTicketType();

            if (filterSearch.Type is not null)
            {
                query = query.Where(u => u.TicketType == filterSearch.Type);
            }
            if(filterSearch.Status is not null)
            {
                query = query.Where(u => u.Status == filterSearch.Status);
            }
            if(filterSearch.TRN is not null)
            {
                query = query.Where(u=>u.TRN == filterSearch.TRN);
            }
            if(filterSearch.Date is not null)
            {
                var split= filterSearch.Date.Split("to");
                var from = Convert.ToDateTime(split[0]);
                var to = Convert.ToDateTime(split[1]);
                query = query.Where(u => u.CreatedAt >= from && u.CreatedAt <= to);
            }
            result.AllTickets = query.ToList();
            result.Status = filterSearch.Status;
            result.TRN = filterSearch.TRN;
            result.Type = filterSearch.Type;
            result.Date = filterSearch.Date;

            return View(result);

        }
        private async void BindTicketStatus()
        {
            var status = await _ticket.GetStatusListAsync();

            ViewBag.Status = new SelectList(status.ToList(), "Id", "Status");

        }
        private async void BindTicketType()
        {
            var type = await _ticket.GetTypeListAsync();
            
            ViewBag.TicketType = new SelectList(type.ToList(), "Id", "Type");
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string trn)
        {
            if (string.IsNullOrEmpty(trn))
            {
                ViewBag.Error = "Invalid TRN.";
                return View(new TicketDTO());
            }

            var result = await _ticket.GetTicketDetailAsync(trn);

            if (!result.Success || result.Data == null)
            {
                ViewBag.Error = result.Message ?? "Ticket not found.";
                return View(new TicketDTO());
            }

            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(TicketCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid comment data.";
                return RedirectToAction("Detail", new { trn = request.TRN });
            }
            if (!Request.Form.ContainsKey("IsVisibleToCustomer"))
            {
                request.IsVisibleToCustomer = false;
            }
            var userName = User.Identity?.Name ?? "Admin";

            try
            {
                await _ticket.SaveTicketCommentAsync(request, userName);
                TempData["Success"] = "Comment added successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Detail", new { trn = request.TRN });
        }
    }
}
