using Azure;
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
using Microsoft.AspNetCore.Http;
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

        #region main inventory from eSim Go

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

        #endregion

        #region add bundle detail in subscribers inventory

        public async Task<Result<string>> AddBundleInventoryAsync(CreateOrderResponse input, string subscriberId)
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

        #endregion

        #region get list of bundles from subscribers inventory

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

                    result.StatusCode = StatusCodes.Status200OK;
                    result.Data = bundleList;
                }
                else
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.Success = false;
                    result.Message = "Not found the Inventory";
                }

                return result;
            }
            catch (Exception ex)
            {
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.Success = false;
                result.Message = "something went wrong";
                result.Data = null;

                return result;
            }
        }

        #endregion

        #region refund bundle from subscribers inventory
        public async Task<Result<string>> RefundBundleAsync(RefundBundleDataBaseRequest input, string subscriberId)
        {
            Result<string> result = new Result<string>();
            var subID = Guid.Parse(subscriberId);

            try
            {
                //find the selected bundle form database against the subscriber
                var subscriberBundls = _db.SubscribersInventory
                    .FirstOrDefault(x => x.SubscriberId == subID && x.Item == input.Item);

                if (subscriberBundls == null)
                {
                    result.StatusCode = StatusCodes.Status404NotFound;
                    result.Success = false;
                    result.Message = "No Bundle Found From Inventory";
                    return result;
                }


                //only one bundle will be refunded

                if (subscriberBundls.Quantity < input.Quantity || input.Quantity > 1)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.Success = false;
                    result.Message = "Quantity Mismatch";
                    return result;
                }

                //get all the available bundles from inventory

                Result<GetBundleInventoryResponse> inventory = await GetBundleInventoryAsync();

                if (!inventory.Success || inventory.Data == null)
                {
                    result.StatusCode = StatusCodes.Status404NotFound;
                    result.Success = false;
                    result.Message = "something went wrong";
                    return result;
                }

                var matchedBundle = inventory.Data.Bundles
                    .FirstOrDefault(b => b.Name == subscriberBundls.Item);

                if (matchedBundle == null)
                {
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.Success = false;
                    result.Message = "There is no bundle in inventory";
                    return result;
                }


                //Executing the refund bundle Method

                RefundBundleInventoryRequest request = new RefundBundleInventoryRequest()
                {
                    usageId = matchedBundle.Available.FirstOrDefault()?.Id,
                    quantity = matchedBundle.Available.FirstOrDefault()?.Total
                };

                var refundedBundle = await RefundAsync(request);

                if (refundedBundle.Success)
                {
                    if (subscriberBundls.Quantity > 1)
                    {
                        //when bundle quantity is more than one
                        subscriberBundls.Quantity -= 1;
                        _db.SubscribersInventory.Update(subscriberBundls);
                    }

                    else
                    {
                        //delete the bundle from subscribers inventory when quantity is one
                        _db.SubscribersInventory.Remove(subscriberBundls);
                    }


                    await _db.SaveChangesAsync();


                    result.Message = refundedBundle.Message;
                    result.StatusCode = refundedBundle.StatusCode;
                    return result;
                }

                result.Success = false;
                result.Message = refundedBundle.Message;
                result.StatusCode = refundedBundle.StatusCode;
                return result;

            }
            catch (Exception ex)
            {
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }


        //calling refund Api of eSim Go

        private async Task<Result<string>> RefundAsync(RefundBundleInventoryRequest request)
        {
            Result<string> result = new();
            var Url = $"{BusinessManager.BaseURL}/inventory/refund";

            try
            {
                var response = await _consume.PostApi<RefundBundleResponse, RefundBundleInventoryRequest>(Url, request);

                if (response.Status != null && response.Status.Contains("Successfully", StringComparison.OrdinalIgnoreCase))
                {
                    result.Message = response.Status;
                    result.StatusCode = StatusCodes.Status200OK;
                    return result;
                }

                result.Success = false;
                result.Message = response.Status;
                result.StatusCode = StatusCodes.Status400BadRequest;
                return result;

            }
            catch (Exception ex)
            {
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }
        #endregion

    }
}
