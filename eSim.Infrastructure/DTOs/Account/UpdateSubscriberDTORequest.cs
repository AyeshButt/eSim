using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Account
{
    public class UpdateSubscriberDTORequest
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
    }
}
