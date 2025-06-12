using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Account
{
    public class ProfileImageDTORequest
    {
        public Guid SubscriberId { get; set; }
        public string? ProfileImage { get; set; }
    }
}
