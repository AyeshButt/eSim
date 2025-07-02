using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Selfcare.Bundles
{
    public class OrderModalViewModel
    {
        public string BundleName { get; set; } = null!;
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
