using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class GetEsimInstallDetailReponseDTO
    {
        public string iccid { get; set; }
        public string matchingId { get; set; }
        public string smdpAddress { get; set; }
        public string profileStatus { get; set; }
        public string pin { get; set; }
        public string puk { get; set; }
        public DateTime firstInstalledDateTime { get; set; }
    }
}
