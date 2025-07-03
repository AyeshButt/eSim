using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Middleware.Bundle;

namespace eSim.Infrastructure.DTOs.Selfcare.Bundle
{
    public class BundleViewModel : GetBundleCatalogueResponse
    {
        public string CountryName { get; set; }
        public string Iso3 { get; set; }
        public string Iso2 { get; set; }

    }
}
