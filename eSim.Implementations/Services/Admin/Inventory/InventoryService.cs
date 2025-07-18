using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.AccessControl;
using eSim.Infrastructure.DTOs.Admin.Inventory;
using eSim.Infrastructure.DTOs.Subscribers;
using eSim.Infrastructure.Interfaces.Admin.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Implementations.Services.Admin.Inventory
{
    public class InventoryService : IInventory
    {
        private readonly ApplicationDbContext _db;
        public InventoryService(ApplicationDbContext db)
        {
            _db = db;
        }
        public UserTempDTO GetUsers()
        {
            var groups = _db.ApplicationUser.Where(u => u.UserType > 1).Select(a=> new ApplicationUserDTOTemporay { 
            
                UserId = a.Id,
                Email =   a.Email,
                ParentId = a.ParentId,
                Username   = a.UserName,
                UserRoleId = a.UserRoleId,
                UserType = a.UserType
            
            }).GroupBy(u => u.ParentId);
            //return groups;

            var listOfParents = _db.ApplicationUser.GroupBy(u => u.ParentId).Select(a => a.Key).ToList();
            var uses = _db.ApplicationUser.Select(a => new ApplicationUserDTOTemporay
            {

                UserId = a.Id,
                Email = a.Email,
                ParentId = a.ParentId,
                Username = a.UserName,
                UserRoleId = a.UserRoleId,
                UserType = a.UserType

            });

            return new UserTempDTO { ParentKeys = listOfParents, Users = uses }; 
             
        }
        public async Task<IQueryable<SubscriberDTO>> GetClientSubscribersAsync(string clientId)
        {
            GetUsers();
            Guid parsedClientId = Guid.Parse(clientId);

            var client_subscriber_list = (from s in _db.Subscribers
                                          join c in _db.Client on s.ClientId equals c.Id
                                          where s.ClientId == parsedClientId
                                          select new SubscriberDTO()
                                          {
                                              Id = s.Id,
                                              FirstName = s.FirstName,
                                              LastName = s.LastName,
                                              Email = s.Email,
                                              Hash = s.Hash,
                                              Active = s.Active,
                                              ClientId = s.ClientId,
                                              CreatedAt = s.CreatedAt,
                                              ModifiedAt = s.ModifiedAt,
                                          }
                                  );


            return await Task.FromResult(client_subscriber_list);
        }
        public async Task<IQueryable<AdminInventoryDTO>> GetInventoryAsync()
        {
            var inventory = (from a in _db.SubscribersInventory
                             join b in _db.Subscribers on a.SubscriberId equals b.Id
                             join c in _db.Client on b.ClientId equals c.Id
                             select new AdminInventoryDTO
                             {
                                 Client = c.Name,
                                 ClientId = c.Id,
                                 Subscriber = $"{b.FirstName} {b.LastName}",
                                 Id = a.Id,
                                 SubscriberId = a.SubscriberId,
                                 OrderRefrenceId = a.OrderRefrenceId,
                                 Type = a.Type,
                                 Item = a.Item,
                                 Description = a.Description,
                                 DataAmount = a.DataAmount,
                                 Duration = a.Duration,
                                 Country = a.Country,
                                 Quantity = a.Quantity,
                                 CreatedDate = a.CreatedDate,
                                 Assigned = a.Assigned,
                                 AllowReassign = a.AllowReassign,
                             });
            return await Task.FromResult(inventory);
        }
    }
}
