using Azure;
using eSim.Common.Enums;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware.Inventory;
using eSim.Infrastructure.Interfaces.Middleware.Order;
using Newtonsoft.Json;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Implementations.Services.Middleware.Order
{
    public class OrderService : IOrder
    {
        private readonly ApplicationDbContext _db;
        private readonly IConsumeApi _consume;
        private readonly IEmailService _email;
        private readonly IInventory _inventory;

        public OrderService(ApplicationDbContext db, IConsumeApi consume, IEmailService email, IInventory inventory)
        {
            _db = db;
            _consume = consume;
            _email = email;
            _inventory = inventory;
        }
        public async Task<Result</*CreateOrderResponse*/ GetOrderDetailResponse>> CreateOrderAsync(CreateOrderRequest input,string subscriberId)
        {
            var result = new Result</*CreateOrderResponse*/ GetOrderDetailResponse>();
            var orderRefId = "5a38e795-0112-42dd-8b13-af95c49d3370";

            string url = $"{BusinessManager.BaseURL}/orders";

            Orders order = new();
            OrderDetail orderDetail = new();
            GetOrderDetailResponse orderDetailResponse = new();

            try
            {
                //var response = await _consume.PostApi<CreateOrderResponse, CreateOrderRequest>(url, input);

                order.SubscriberId = Guid.Parse(subscriberId);
                order.Id = Guid.NewGuid();
                order.OrderReferenceId = orderRefId;

                await _db.Orders.AddAsync(order);
                await _db.SaveChangesAsync();

                var response = await GetOrderDetailAsync(orderRefId);
                
                
                if (response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;

                    return result;
                }

                //if (!response.Order.Any())
                if (!response.Data.Order.Any())
                {
                    result.Success = false;
                    result.Message = response.Message;
                }
                else
                {
                    orderDetailResponse = response.Data;

                    order.Total = response.Data.Total;
                    order.Assigned = response.Data.Assigned ?? false;
                    order.CreatedDate = response.Data.CreatedDate;
                    order.Currency = response.Data.Currency;
                    order.SourceIP = response.Data.SourceIP;
                    order.Status = response.Data.Status;
                    order.StatusMessage = response.Data.StatusMessage;

                    _db.Orders.Update(order);
                    await _db.SaveChangesAsync();

                    var orderDetails = response.Data.Order.Select(u => new OrderDetail()
                    {
                        Id = Guid.NewGuid(),
                        OrderReferenceId = response.Data.OrderReference,
                        Type = u.Type,
                        Item = u.Item,
                        Quantity = u.Quantity,
                        SubTotal = u.SubTotal,
                        PricePerUnit = u.PricePerUnit,
                        AllowReassign = u.AllowReassign,
                    });

                    await _db.OrderDetails.AddRangeAsync(orderDetails);
                    
                    await _db.SaveChangesAsync();

                    await _inventory.AddBundleInventoryAsync(orderDetailResponse, subscriberId);

                    result.Data = response.Data;
                }
            }
            catch (Exception ex)
            {
                var email = new EmailDTO()
                {
                    To = "ayeshbutt4321@gmail.com",
                    Subject = "Exception while dumping db data",
                    Body = GeneratePlainTextOrderSummary(orderDetailResponse)
                };
                _email.SendEmail(email);
                result.Message = ex.Message;
            }

            return result;
        }
        public async Task<Result<GetOrderDetailResponse>> GetOrderDetailAsync(string orderReferenceId)
        {
            var result = new Result<GetOrderDetailResponse>();

            string url = $"{BusinessManager.BaseURL}/orders/{orderReferenceId}";
            try
            {
                result = await _consume.GetApii<GetOrderDetailResponse>(url);

                if (result.Success)
                {
                    //perform db related operations
                }
                
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<Result<ListOrderResponse>> ListOrderAsync(ListOrderRequest input)
        {
            var result = new Result<ListOrderResponse>();

            string dateFilter = string.Empty;

            if (input.From is not null && input.To is not null)
            {
                dateFilter = BusinessManager.DateFilterQueryBuilder(input.From.Value.Date, input.To.Value.Date);
            }

            string url = $"{BusinessManager.BaseURL}/orders?page={input.Page}&limit={input.Limit}&{dateFilter}&includeIccids={input.IncludeIccids}";

            try
            {
                var response = await _consume.GetApi<ListOrderResponse>(url);

                if (response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;

                    return result;
                }

                if (!response.Orders.Any())
                {
                    result.Success = false;
                    result.Message = BusinessManager.NoOrderPresent;
                }
                response.Orders.OrderByDescending(u => u.CreatedDate);

                result.Data = response;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

            return result;
        }

        #region Order response mapped to email body (to be refined)
        public string GeneratePlainTextOrderSummary(GetOrderDetailResponse response)
        {
            var sb = new StringBuilder();

            sb.AppendLine("ORDER SUMMARY");
            sb.AppendLine("------------------------------");
            sb.AppendLine($"Order Reference: {response.OrderReference}");
            sb.AppendLine($"Created Date   : {response.CreatedDate:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Status         : {response.Status}");
            sb.AppendLine($"Status Message : {response.StatusMessage}");
            sb.AppendLine($"Total Amount   : {response.Total} {response.Currency}");
            sb.AppendLine($"Assigned       : {response.Assigned}");
            sb.AppendLine($"Source IP      : {response.SourceIP}");
            sb.AppendLine($"Message        : {response.Message}");
            sb.AppendLine($"Type           : {response.Currency}");
            sb.AppendLine();
            sb.AppendLine("ORDERED ITEMS:");
            sb.AppendLine("Type\tItem\tQty\tUnitPrice\tSubTotal\tReassign");
            sb.AppendLine("------------------------------------------------------");

            foreach (var item in response.Order)
            {
                sb.AppendLine($"{item.Type}\t{item.Item}\t{item.Quantity}\t{item.PricePerUnit}\t{item.SubTotal}\t{item.AllowReassign}");
            }

            return sb.ToString();
        }
        #endregion
    }
}
