using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Middleware;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface IAuthService
    {
        string? Authenticate(AuthDTO input);
    }
}
