using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class InventoryAvailableBundle
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InventoryBundleId { get; set; }
        //new comment
        //confusion on this prop
        public int AvailableId { get; set; }
        public int Total { get; set; }
        public int Remaining { get; set; }
        public string Expiry { get; set; }
    }
}
