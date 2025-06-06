using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;

namespace eSim.Common.Extensions
{
    public static class Extensions
    {
        public static string? SubscriberId(this ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(u => u.Type == BusinessManager.SubscriberId)?.Value;
        }
    }
}
