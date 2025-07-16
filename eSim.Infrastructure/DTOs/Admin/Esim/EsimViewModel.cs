using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Subscribers;

namespace eSim.Infrastructure.DTOs.Admin.Esim
{
    public class EsimViewModel
    {
        public string? Client { get; set; }
        public string? Subscriber { get; set; }
        public string? DateRange { get; set; }
        public string? Iccid { get; set; }
        public List<EsimsList> EsimsResponseList { get; set; } = new List<EsimsList>();
    }

    public class EsimsList
    {
        public string? Id { get; set; }
        public string SubscriberId { get; set; } = null!;
        public string? SubscriberName { get; set; }
        public string? Iccid { get; set; }
        public string? CustomerRef { get; set; }
        public string? LastAction { get; set; }
        public DateTime? ActionDate { get; set; }
        public bool? Physical { get; set; }
        public DateTime? AssignedDate { get; set; }

        public string? Client { get; set; }
    }
}
