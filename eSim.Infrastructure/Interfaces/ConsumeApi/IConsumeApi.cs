using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.ConsumeApi
{
    public interface IConsumeApi
    {
        Task<T?> GetApi<T>(string url);
        Task<Result<T>> GetApii<T>(string url);
        Task<T?> PostApi<T, I>(string url, I data);
        Task<T?> PutApi<T, I>(string url, I data);

    }
}
