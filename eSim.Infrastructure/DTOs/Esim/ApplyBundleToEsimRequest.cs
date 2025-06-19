using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class EsimData
    {
        public string Iccid { get; set; }
        public string Status { get; set; }
        public string Bundle { get; set; }
    }
    public class ApplyBundleToEsimResponse
    {
        public List<EsimData> Esims { get; set; }
        public string ApplyReference { get; set; }
        public string? Message { get; set; }
    }

    public class BundlePayload
    {
        public string Name { get; set; } = null!;
        public int? Repeat { get; set; }
        public bool? AllowReassign { get; set; }
    }
    public class ApplyBundleToExistingEsimRequest
    {
        [Required]
        public string Iccid { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        public bool AllowReassign { get; set; } = false;

    }
    public class ApplyBundleToEsimRequest
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
