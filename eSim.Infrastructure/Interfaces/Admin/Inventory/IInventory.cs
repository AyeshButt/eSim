using eSim.Infrastructure.DTOs.Admin.Inventory;
using eSim.Infrastructure.DTOs.Subscribers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.Admin.Inventory
{
    public interface IInventory
    {
        public Task<IQueryable<AdminInventoryDTO>> GetInventoryAsync();
        public Task<IQueryable<SubscriberDTO>> GetClientSubscribersAsync(string clientId);
    }
}
