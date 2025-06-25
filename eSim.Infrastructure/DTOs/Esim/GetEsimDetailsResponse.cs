using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class GetEsimDetailsResponse
    {
        public string? Iccid { get; set; }
        public string? Pin { get; set; }
        public string? Puk { get; set; }
        public string? MatchingId { get; set; }
        public string? SmdpAddress { get; set; }
        public string? ProfileStatus { get; set; }
        public long? FirstInstalledDateTime { get; set; }
        public string? CustomerRef { get; set; }
        public string? AppleInstallUrl { get; set; }
    }
}
