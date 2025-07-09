using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Subscribers
{
    public class GetCountryResponse
    {

        public Guid CountryId { get; set; }
        public string Country { get; set; }
    }
}
