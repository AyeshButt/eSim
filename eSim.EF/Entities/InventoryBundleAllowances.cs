using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class InventoryBundleAllowances
    {
        [Key]
        public Guid Id { get; set; }
        //new comment

        public Guid InventoryBundleId { get; set; }
        public string Type { get; set; }
        public string Service { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
        public bool Unlimited { get; set; }
    }
}
