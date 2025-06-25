using eSim.Infrastructure.DTOs.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class EsimDetailsPartialViewModel
    {
        public Result<GetEsimDetailsResponse>? EsimDetails { get; set; }
        public Result<GetEsimHistoryResponse>? EsimBundles { get; set; }
        public Result<GetEsimHistoryResponse>? EsimHistory { get; set; }

    }
}
