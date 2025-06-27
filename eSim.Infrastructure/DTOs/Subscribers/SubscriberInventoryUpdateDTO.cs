using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Subscribers
{
    public class SubscriberInventoryUpdateDTO
    {
        public required string OrderReference { get; set; }
        public required bool Assigned { get; set; }
        public required DateTime CreatedDate { get; set; }
    }
}
