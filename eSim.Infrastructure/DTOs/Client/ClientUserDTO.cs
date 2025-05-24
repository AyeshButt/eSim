using eSim.Infrastructure.DTOs.AccessControl;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Client
{
    public class ClientUserDTO
    {
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public object? User { get; set; }
        public string? Token { get; set; }
    }
}
