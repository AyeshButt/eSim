﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Account
{
    public class CountriesDTORequest
    {
        public string CountryName { get; set; }
        public string Iso3 { get; set; }
        public string Iso2 { get; set; }

        public int RegionId { get; set; }
    }
}
