using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Infrastructure.DTOs.Selfcare.Inventory
{
    public class SubscriberInventoryResponseViewModel : GetBundleCatalogueDetailsResponse
    {
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Assigned { get; set; }  
    }
}
