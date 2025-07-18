using System;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Admin.Esim;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Subscribers;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Admin.Esim;
using eSim.Infrastructure.Interfaces.Admin.Order;
using eSim.Infrastructure.Interfaces.Middleware;
using eSim.Infrastructure.Interfaces.Middleware.Esim;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eSim.Admin.Controllers
{
    public class SimController : Controller
    {
        private readonly IEsims _esim;
        private readonly ISubscriberService _subscriber;
        private readonly IClient _client;

        public SimController( IEsims esim, ISubscriberService subscriber, IClient client)
        {
            _esim = esim;
            _subscriber = subscriber;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index( EsimViewModel input)
        {
            EsimViewModel model = new();
            var clients = await _client.GetAllClientsAsync();
            var subScribers = await _subscriber.GetClient_SubscribersListAsync();

            if (subScribers is null || clients is null)
                return View(model);

            var esimsList = await _esim.GetEsimListForAllSubscribersAsync();

            model.EsimsResponseList = FilterEsims(esimsList, input).ToList();

            ViewBag.Clients = clients.Select(u => new ClientDTO() { Name = u.Name, Id = u.Id }).ToList();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> GetClientSubscribers(string clientId)
        {
            var subscribers = await _subscriber.GetClient_SubscribersListByID(clientId);

            if (subscribers is null)
                return Json(new List<SelectListItem>());

            var model = subscribers.Select(u => new SelectListItem() { Text = u.Email, Value = u.Id.ToString() }).ToList();

            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {

            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TabPartialViews(string tab, string iccid) 
        {
            switch (tab)
            {
                case "Detail":
                    var esimdetail = await _esim.GetEsimDetailsAsync(iccid);
                    return PartialView("_EsimDetailsPartialView", esimdetail.Data);

                case "AppliedBundles":
                    var appliedBundles = await _esim.GetListBundlesAppliedToEsimAsync(iccid);
                    return PartialView("_EsimAppliedBundlesPartailView", appliedBundles.Data);

                default:
                    var esimHistory = await _esim.GetEsimHistoryAsync(iccid);
                    return PartialView("_EsimHistoryPartialView", esimHistory.Data);
            }
            
        }


        private IQueryable<EsimsList> FilterEsims(IQueryable<EsimsList> esims, EsimViewModel input)
        {
            if (input == null) return esims;

            // Date Range Filter
            if (!string.IsNullOrWhiteSpace(input.DateRange))
            {
                var dates = input.DateRange.Split(new[] { " to " }, StringSplitOptions.RemoveEmptyEntries);

                if (dates.Length == 2 &&
                    DateTime.TryParse(dates[0], out var fromDate) &&
                    DateTime.TryParse(dates[1], out var toDate))
                {
                    var to = toDate.Date.AddDays(1).AddSeconds(-1);
                    esims = esims.Where(u => u.AssignedDate >= fromDate && u.AssignedDate <= to);
                }
            }

            // Client Filter
            if (!string.IsNullOrWhiteSpace(input.Client))
            {

                esims = esims.Where(u => u.Client == input.Client);
            }

            // iccid
            if (!string.IsNullOrWhiteSpace(input.Iccid))
            {

                esims = esims.Where(u => u.Iccid == input.Iccid);
            }

            // Subscriber Filter
            if (!string.IsNullOrWhiteSpace(input.Subscriber))
            {
                esims = esims.Where(u => u.SubscriberId == input.Subscriber);
            }

            return esims; // Don't forget to return the filtered query
        }

    }
}
