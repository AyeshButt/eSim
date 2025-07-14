using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Admin.order;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.Admin.Order;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Raven.Database.Indexing.IndexingWorkStats;

namespace eSim.Implementations.Services.Admin.Order
{
    public class AdminOrderService : IAdminOrder
    {
        private readonly ApplicationDbContext _db;
        public AdminOrderService(ApplicationDbContext db)
        {
            _db = db;
        }



        public async Task<Result<GetOrderDetailResponse>> GetOrderDetailAsync(string orderReferenceId)
        {
            var result = new Result<GetOrderDetailResponse>();

            try
            {
                var order = await _db.Orders
                    .Where(o => o.OrderReferenceId == orderReferenceId)
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    result.Success = false;
                    result.Message = "Order not found.";
                    return result;
                }

                result.Data = new GetOrderDetailResponse
                {
                    OrderReference = order.OrderReferenceId,
                    Total = order.Total ?? 0,
                    Currency = order.Currency,
                    Status = order.Status,
                    StatusMessage = order.StatusMessage,
                    CreatedDate = order.CreatedDate ?? DateTime.MinValue,
                    Assigned = order.Assigned,
                    SourceIP = order.SourceIP
                };

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"An error occurred: {ex.Message}";
            }

            return result;
        }

        public async Task<Result<ListOrderResponse>> GetOrdersFromDbAsync(ListOrderRequest request)
        {
             var result = new Result<ListOrderResponse>();
            try
            {
                var orderList=_db.Orders.AsQueryable();
                if (request.From.HasValue && request.To.HasValue)
                {
                    orderList = orderList.Where(x => x.CreatedDate >= request.From && x.CreatedDate <=request.To);
                }
             
                int Page =request.Page?? 1;
               int limit = request.Limit ?? 10;
                int total = await orderList.CountAsync();
                // using Filter
                var orders = await orderList
               .OrderByDescending(x => x.CreatedDate)
               .Skip((Page - 1) * limit)
               .Take(limit)
               .ToListAsync();


                var response = new ListOrderResponse
                {
                    Orders = orders.Select(order => new GetOrderDetailResponse
                    {
                        OrderReference = order.OrderReferenceId,
                        Total = order.Total ?? 0,
                        Currency = order.Currency,
                        Status = order.Status,
                        StatusMessage = order.StatusMessage,
                        CreatedDate = order.CreatedDate ?? DateTime.MinValue,
                        Assigned = order.Assigned,
                        SourceIP = order.SourceIP,
                        RunningBalance = 0
                    }).ToList(),
                    PageCount = (int)Math.Ceiling((double)total / limit),
                    PageSize = limit,
                    Rows = total
                };
                result.Success = true;
               result.Data= response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<List<OrderDetailResponseViewModel>> GetOrderBySubscriberId(string Id)
        {
            Guid.TryParse(Id, out Guid subId);

            var order = await _db.Orders
                .Where(o => o.SubscriberId == subId)
                .ToListAsync();

            var orderRefId = order.Select(o => o.OrderReferenceId).ToList();

            var orderDetail = await _db.OrderDetails
                .Where(od => orderRefId.Contains(od.OrderReferenceId))
            .ToListAsync();


            var result = order.Select(order => new OrderDetailResponseViewModel
            {
                Total = order.Total,
                OrderReferenceId = order.OrderReferenceId,
                Currency = order.Currency,
                Status = order.Status,
                StatusMessage = order.StatusMessage,
                CreatedDate = order.CreatedDate,
                Assigned = order.Assigned,
                SourceIP = order.SourceIP,

                Order = orderDetail
                .Where(od => od.OrderReferenceId == order.OrderReferenceId)
                .Select(od => new OrderBySubIdDetail
                {
                    Type = od.Type,
                    Item = od.Item,
                    Quantity = od.Quantity,
                    SubTotal = od.SubTotal,
                    PricePerUnit = od.PricePerUnit,
                    AllowReassign = od.AllowReassign
                }).ToList()

            }).ToList();
            
            return result;
            //var ordersWithDetails = await _db.Orders
            //    .Where(o => o.SubscriberId == subId)
            //    .Select( u => new OrderDetailResponseViewModel
            //    {
            //        Total = u.Total,
            //        Currency = u.Currency,
            //        Status = u.Status,
            //        StatusMessage = u.StatusMessage,
            //        CreatedDate = u.CreatedDate,
            //        Assigned = u.Assigned,
            //        SourceIP = u.SourceIP,

            //        Order =  _db.OrderDetails
            //        .Where(od => od.OrderReferenceId == u.OrderReferenceId)
            //        .Select(od => new OrderBySubIdDetail
            //        {
            //            Type = od.Type,
            //            Item  = od.Item,
            //            Quantity = od.Quantity,
            //            SubTotal = od.SubTotal,
            //            PricePerUnit = od.PricePerUnit,
            //            AllowReassign = od.AllowReassign
            //        }).

            //    });
        }
    }
}
