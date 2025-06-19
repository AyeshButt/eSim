using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class EsimCompatibilityRequestDto
    {
        [Required(ErrorMessage = "ICCID is required.")]
        public string Iccid { get; set; }
        [Required(ErrorMessage = "Bundle is required.")]
        public string Bundle { get; set; }
    }
    public class EsimCompatibilityResponseDTO
    {

        public bool Compatible { get; set; }
        public string? Message { get; set; }
    }
   


}
