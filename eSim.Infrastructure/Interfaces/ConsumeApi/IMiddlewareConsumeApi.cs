using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.ConsumeApi
{
    public interface IMiddlewareConsumeApi
    {
        Task<T?> Get<T>(string url);
        Task<T?> AuthPost<T, I>(string url, T data);
        Task<T?> PostApi<T, I>(string url, I data);
        Task<T?> PutApi<T, I>(string url, I data);
    }
}
