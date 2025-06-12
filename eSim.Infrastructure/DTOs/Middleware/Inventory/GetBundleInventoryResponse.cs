using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Inventory
{
    public class Allowance
    {
        public string Type { get; set; } = null!;
        public string Service { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Amount { get; set; }
        public string Unit { get; set; } = null!;
        public bool Unlimited { get; set; }
    }

    public class Available
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public int Remaining { get; set; }
        public string Expiry { get; set; } = null!;
    }

    public class Bundle
    {
        public string Name { get; set; } = null!;
        public string Desc { get; set; } = null!;
        public bool UseDms { get; set; } 
        public List<Available> Available { get; set; }
        public List<string> Countries { get; set; }
        public int Data { get; set; } 
        public int Duration { get; set; }
        public string DurationUnit { get; set; } = null!;
        public bool Autostart { get; set; }
        public bool Unlimited { get; set; }
        public List<string> Speed { get; set; }
        public List<Allowance> Allowances { get; set; }
    }

    public class GetBundleInventoryResponse
    {
        public List<Bundle> Bundles { get; set; } = new();
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int Rows { get; set; }
    }
}
