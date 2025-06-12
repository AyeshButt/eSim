using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class InventoryBundleCountries
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InventoryBundleId { get; set; }
        public string Name { get; set; }
        //new comment

    }
}
