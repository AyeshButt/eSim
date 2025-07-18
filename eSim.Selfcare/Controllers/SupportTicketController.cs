﻿using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Selfcare.Ticket;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.DTOs.Selfcare.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Razor.Generator;

namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class SupportTicketController : Controller
    {
        private readonly ITicketService _ts;

        public SupportTicketController(ITicketService ts)
        {
            _ts = ts;
        }

        #region Get Ticket
        [HttpGet]
        public async Task<IActionResult> List(string? search, string? dateRange, string? status)
        {
            var data = await _ts.Get();

            if (data.Success)
            {
          
                //search on the base of TRN
                if (!string.IsNullOrWhiteSpace(search))
                {
                    data.Data = data.Data.Where(x => x.TRN.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                    return View(data);
                }

                //search on the bases of status
                //if (!string.IsNullOrWhiteSpace(status) && status != "all")
                //{
                //    data.Data = data.Data.Where(x=> x.)
                //}
                return View(data);
            }
            else
            {
                return View(data);
            }   
        }

        #endregion

        #region TicketDetails
        [HttpGet]
        public async Task<IActionResult> TicketDetails(string trn)
        {
            var response = await _ts.Detail(trn);
            return View(response);
        }
        #endregion


        #region Open new Ticket
        [HttpGet]
        public async Task<IActionResult> OpenNewTicket()
        {
            TicketRequestViewModel model = new();

            var data = await _ts.GetTicketType();

            if (data.Success) 
            {
                model.Types = data.Data;
                return View(model); 
            }

            return View(model);
          
        }


        [HttpPost]
        public async Task<IActionResult> OpenNewTicket(TicketRequestViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var resp = await _ts.Create(model);

                if (resp.Success)
                {
                    TempData["ToastMessage"] = resp.Message + " " + "Ticket Has been genrated";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("List");
                }
                else
                {
                    var data = await _ts.GetTicketType();
                    if (data.Success)
                    {
                        model.Types = data.Data;
                    }
                    TempData["ToastMessage"] = resp.Message;
                    TempData["ToastType"] = resp.Success;
                    return View(model);
                }
            }
            else
            {
                var data = await _ts.GetTicketType();
                if (data.Success)
                {
                    model.Types = data.Data;
                }

                return View(model);
            }
        }

        #endregion


        [HttpPost]
        public async Task<IActionResult> PostComment(TicketCommentRequest commentRequest)
        {
            await _ts.PostCommentAsync(commentRequest);
            return RedirectToAction("TicketDetails", new { trn = commentRequest.TRN });
        }

    }
}
