using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Bundle
{
        
    public class Bundle
    {
        public Bundle()
        {
            countries = new List<Country>();
            speed = new List<string>();
            roamingEnabled = new List<RoamingEnabled>();
            groups = new List<string>();
        }

        public string name { get; set; }
        public string description { get; set; }
        public List<Country> countries { get; set; }
        public int dataAmount { get; set; }
        public int duration { get; set; }
        public List<string> speed { get; set; }
        public bool autostart { get; set; }
        public bool unlimited { get; set; }
        public List<RoamingEnabled> roamingEnabled { get; set; }
        public string imageUrl { get; set; }
        public double price { get; set; }
        public List<string> groups { get; set; }
        public string billingType { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string region { get; set; }
        public string iso { get; set; }
    }

    public class RoamingEnabled
    {
        public string name { get; set; }
        public string region { get; set; }
        public string iso { get; set; }
    }

    public class GetBundleCatalogueResponse
    {
        public GetBundleCatalogueResponse()
        {
            bundles = new List<Bundle>();
        }
        public List<Bundle> bundles { get; set; }
        public int pageCount { get; set; }
        public int rows { get; set; }
        public int pageSize { get; set; }
    }


}
