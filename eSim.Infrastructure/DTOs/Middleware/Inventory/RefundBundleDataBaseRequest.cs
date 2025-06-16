using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raven.Database.FileSystem.Storage.Voron.Impl.Tables;

namespace eSim.Infrastructure.DTOs.Middleware.Inventory
{
    public class RefundBundleDataBaseRequest
    {
        //public Guid SubscriberId { get; set; }
        //public string OrderRefrenceId { get; set; }
        public string Item { get; set; }
        public int? Quantity { get; set; }
    }

    public class RefundBundleInventoryRequest
    {
      
        public int? usageId { get; set; }

        public int? quantity { get; set; }
    }

    public class RefundBundleResponse
    {
        public string Status { get; set; }
    }
}
