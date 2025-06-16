using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class EsimsDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SubscriberId { get; set; } = null!;
        public string? Iccid { get; set; }
        public string? CustomerRef { get; set; }
        public string? LastAction { get; set; }
        public DateTime? ActionDate { get; set; }
        public bool? Physical { get; set; }
        public DateTime? AssignedDate { get; set; }
    }
}
