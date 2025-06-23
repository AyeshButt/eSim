using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class GetEsimHistoryResponse
    {
        public List<EsimHistoryActionDTO> Actions { get; set; }
        public int Rows { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }

    }
    public class EsimHistoryActionDTO
    {
        public string Name { get; set; }
        public string BundleName { get; set; }
        public string Date { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AlertType { get; set; } 
    }

}
