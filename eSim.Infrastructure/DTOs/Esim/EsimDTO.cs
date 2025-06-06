using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class GetListofyourEsimsResponseDTO
    {
        public EsimResponseDTO()
        {
            List<EsimDTO> Esims = new();        
        }
        public List<EsimDTO> Esims { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int Rows { get; set; }
    }

    public class EsimDTO
    {
        public string Iccid { get; set; }
        public string CustomerRef { get; set; }
        public string LastAction { get; set; }
        public DateTime ActionDate { get; set; }
        public bool Physical { get; set; }
        public DateTime AssignedDate { get; set; }
    }

}
