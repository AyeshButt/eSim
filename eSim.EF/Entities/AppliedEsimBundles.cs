using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class AppliedEsimBundles
    {
        [Key]
        public string Id { get; set; }
        public string Iccid { get; set; }
        public string BundleName { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
