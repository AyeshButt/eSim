using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class Esims
    {
        [Key]
        public string Id { get; set; }
        public string SubscriberId { get; set; }
        public string Iccid { get; set; }
        public string CustomerRef { get; set; }
        public string LastAction { get; set; }
        public DateTime ActionDate { get; set; }
        public bool Physical { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
