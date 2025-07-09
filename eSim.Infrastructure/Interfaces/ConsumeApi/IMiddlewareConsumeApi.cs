using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.QRDownload;
using Microsoft.AspNetCore.Http;

namespace eSim.Infrastructure.Interfaces.ConsumeApi
{
    public interface IMiddlewareConsumeApi
    {
        Task<Result<T?>> Get<T>(string url);
        Task<Result<T?>> Post<T, I>(string url, I? data);
        Task<Result<T?>> Put<T, I>(string url, I? data);
        Task<FileDownloadResult> DownloadQrCodeAsync(string url);
        Task<Result<T?>> PostMultipartAsync<T>(string url, IFormFile file);


        Task<byte[]> GetQR(string url);


    }
}
