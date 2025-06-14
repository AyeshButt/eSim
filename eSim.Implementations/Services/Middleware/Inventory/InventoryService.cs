using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Inventory;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware;
using eSim.Infrastructure.Interfaces.Middleware.Inventory;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Implementations.Services.Middleware.Inventory
{
    public class InventoryService : IInventory
    {
        private readonly ApplicationDbContext _db;
        private readonly IBundleService _bdService;
        private readonly IEmailService _email;
        private readonly IConsumeApi _consume;

        public InventoryService(IConsumeApi consume, ApplicationDbContext db, IBundleService bdService, IEmailService email)
        {
            _consume = consume;
            _db = db;
            _bdService = bdService;
            _email = email;
        }

        public async Task<Result<GetBundleInventoryResponse>> GetBundleInventoryAsync()
        {
            var result = new Result<GetBundleInventoryResponse>();

            string url = $"{BusinessManager.BaseURL}/inventory";

            try
            {
                var response = await _consume.GetApi<GetBundleInventoryResponse>(url);

                if (response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;

                    return result;
                }

                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<Result<string>> AddBundleInventoryAsync(GetOrderDetailResponse input, string subscriberId)
        {
            Result<string> result = new();

            List<SubscribersInventory> BulkInsert = new List<SubscribersInventory>();
            try
            {
                if (input != null)
                {
                    foreach (var name in input.Order)
                    {
                        Result<GetBundleCatalogueDetailsResponse> bundle = await _bdService.GetBundleDetailsAsync(name.Item);

                        if (bundle.Success)
                        {
                            BulkInsert.Add(new SubscribersInventory
                            {

                                Id = Guid.NewGuid(),
                                SubscriberId = Guid.Parse(subscriberId),
                                OrderRefrenceId = input.OrderReference,
                                Quantity = name.Quantity,
                                Assigned = input.Assigned,
                                AllowReassign = name.AllowReassign,
                                Type = name.Type,
                                Item = name.Item,
                                Description = bundle.Data.description,
                                Duration = bundle.Data.duration,
                                DataAmount = bundle.Data.dataAmount,
                                Country = bundle.Data.countries.FirstOrDefault()?.country.name,
                                CreatedDate = input.CreatedDate,

                            });                     
                        } 
                    }

                    await _db.SubscribersInventory.AddRangeAsync(BulkInsert);
                    await _db.SaveChangesAsync();

                    return result;
                }
                result.Success = false;
                return result;
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Bundle Inventory details of a subscriber");
                sb.AppendLine("-----------------");
                sb.AppendLine(JsonConvert.SerializeObject(BulkInsert));

                var email = new EmailDTO()
                {
                    To = "ayeshbutt4321@gmail.com",
                    Subject = "Exception while dumping db data",
                    Body = sb.ToString()
                };
                _email.SendEmail(email);
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<Result<List<SubscriberInventoryResponse>>> GetSubscriberInventoryResponse(string subscriberId)
        {
            Result<List<SubscriberInventoryResponse>> result = new();

            try
            {
                var subID = Guid.Parse(subscriberId);
                var Inventory = await _db.SubscribersInventory.Where(b => b.SubscriberId == subID).ToListAsync();

                if (Inventory != null)
                {
                    var bundleList = Inventory.Select(o => new SubscriberInventoryResponse
                    {
                        SubscriberId = o.SubscriberId,
                        OrderRefrenceId = o.OrderRefrenceId,
                        Item = o.Item,
                        Description = o.Description,
                        Duration = o.Duration,
                        DataAmount = o.DataAmount,
                        Quantity = o.Quantity,
                        Assigned = o.Assigned,
                        AllowReassign = o.AllowReassign,
                        Type = o.Type,
                        CreatedDate = o.CreatedDate,
                    }).ToList();

                    result.Data = bundleList;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Not found the Inventory";
                }
                    return result;
            }
            catch (Exception ex) 
            { 
                result.Success=false;
                result.Message = "something went wrong";
                return result;
            }
        }
    }
}
