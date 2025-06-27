using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class SubscriberInventoryViewModel
    {
        public string Iccid { get; set; } = null!;
        [Required]
        public string Bundle { get; set; } = null!;
        public List<SelectListItem> Inventory { get; set; } = new();
    }
}
