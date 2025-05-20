using System;
using System.Collections.Generic;

namespace eSim.Infrastructure.DTOs.Middleware.Bundle
{
    public class GetBundleCatalogueDetailDTO
    {
        
            public class Allowance
            {
                public string type { get; set; }
                public string service { get; set; }
                public string description { get; set; }
                public int amount { get; set; }
                public string unit { get; set; }
                public bool unlimited { get; set; }
            }

            public class CountryInfo
            {
                public string name { get; set; }
                public string region { get; set; }
                public string iso { get; set; }
            }

            public class Network
            {
                public string name { get; set; }
                public string brandName { get; set; }
                public List<string> speeds { get; set; }
            }

            public class Country
            {
                public CountryInfo country { get; set; }
                public List<Network> networks { get; set; }  
                public List<Network> potentialNetworks { get; set; }  
            }

            public class RoamingEnabled
            {
                public CountryInfo country { get; set; }
                public List<Network> networks { get; set; }
                public List<Network> potentialNetworks { get; set; }
            }

            public class Speed
            {
                public List<string> speeds { get; set; }  // Changed to List<string>
                public List<string> potentialSpeeds { get; set; }
            }

            public class GetBundleCatalogueDetail
            {
                public string name { get; set; }
                public string description { get; set; }
                public List<Country> countries { get; set; }
                public int dataAmount { get; set; }
                public int duration { get; set; }
                public Speed speed { get; set; }
                public bool autostart { get; set; }
                public bool unlimited { get; set; }
                public List<RoamingEnabled> roamingEnabled { get; set; }
                public string imageUrl { get; set; }
                public double price { get; set; }
                public List<string> group { get; set; }
                public List<Allowance> allowances { get; set; }
                public string billingType { get; set; }
            }
        }
    }
