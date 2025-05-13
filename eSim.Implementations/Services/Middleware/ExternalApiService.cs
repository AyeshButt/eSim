using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.Interfaces.Middleware;

namespace eSim.Implementations.Services.Middleware
{
    public class ExternalApiService : IExternalApiService
    {
        public Task<string?> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }
    }
}

