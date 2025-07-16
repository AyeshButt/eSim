using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Admin.Esim
{
    public class EsimViewModel
    {
        public string Id { get; set; }
        public string SubscriberId { get; set; } = null!;
        public string SubscriberName { get; set; } 
        public string? Iccid { get; set; }
        public string? CustomerRef { get; set; }
        public string? LastAction { get; set; }
        public DateTime? ActionDate { get; set; }
        public bool? Physical { get; set; }
        public DateTime? AssignedDate { get; set; }
    }
}
