using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface IExternalApiService
    {
        string? GetOrders();
    }
}
