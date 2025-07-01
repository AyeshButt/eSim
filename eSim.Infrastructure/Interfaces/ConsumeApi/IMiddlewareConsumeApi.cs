using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.ConsumeApi
{
    public interface IMiddlewareConsumeApi
    {
        Task<Result<T?>> Get<T>(string url);
        Task<Result<T?>> Post<T, I>(string url, I? data);
        Task<byte[]> GetQR(string url);


    }
}
