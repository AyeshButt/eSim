using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.BaseResponse
{
    public class BaseResponseDTO
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
