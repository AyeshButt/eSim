using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Esim;

namespace eSim.EF.Entities
{
    public class InventoryBundles
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SubscriberId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool UseDms { get; set; }
        public int Data { get; set; }
        public int Duration { get; set; }
        public string DurationUnit { get; set; }
        public bool Autostart { get; set; }
        public bool Unlimited { get; set; }

    }
}
