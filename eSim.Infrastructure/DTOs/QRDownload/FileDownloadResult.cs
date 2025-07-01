using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.QRDownload
{
    public class FileDownloadResult
    {
        public byte[]? FileBytes { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? Message { get; set; }        
        public bool Success { get; set; }
    }
}
