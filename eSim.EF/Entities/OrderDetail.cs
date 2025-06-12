using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }
        public string OrderReferenceId { get; set; }
        public string Type { get; set; }
        //new comment

        public string Item { get; set; }
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
        public double PricePerUnit { get; set; }
        public bool AllowReassign { get; set; }
    }
}
