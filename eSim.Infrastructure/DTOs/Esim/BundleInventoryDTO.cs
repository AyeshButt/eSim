using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Esim
{
    public class BundleInventoryDTO
    {
        public List<Bundle> bundles { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public int rows { get; set; }
    }
        public class Allowance
        {
            public string type { get; set; }
            public string service { get; set; }
            public string description { get; set; }
            public int amount { get; set; }
            public string unit { get; set; }
            public bool unlimited { get; set; }
        }

        public class Available
        {
            public int id { get; set; }
            public int total { get; set; }
            public int remaining { get; set; }
            public string expiry { get; set; }
        }

        public class Bundle
        {
            public string name { get; set; }
            public string desc { get; set; }
            public bool useDms { get; set; }
            public List<Available> available { get; set; }
            public List<string> countries { get; set; }
            public int data { get; set; }
            public int duration { get; set; }
            public string durationUnit { get; set; }
            public bool autostart { get; set; }
            public bool unlimited { get; set; }
            public List<string> speed { get; set; }
            public List<Allowance> allowances { get; set; }
        }

       

    
}
