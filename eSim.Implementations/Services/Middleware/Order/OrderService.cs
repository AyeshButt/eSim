using Azure;
using eSim.Common.Enums;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware.Order;
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

        public OrderService(ApplicationDbContext db, IConsumeApi consume)
        {
            _db = db;
            _consume = consume;
        }
        public async Task<Result<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest input)
        {
            var result = new Result<CreateOrderResponse>();

            string url = $"{BusinessManager.BaseURL}/orders";

            try
            {
                var response = await _consume.PostApi<CreateOrderResponse, CreateOrderRequest>(url, input);

                if (response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;

                    return result;
                }

                if (!response.Order.Any())
                {
                    result.Success = false;
                    result.Message = response.Message;
                }
                else
                {
                    //db entry

                    result.Data = response;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
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
                var response = await _consume.GetApi<GetOrderDetailResponse>(url);

                if(response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;

                    return result;
                }

                if(response.Message is not null)
                {
                    result.Message = response.Message;
                    result.Success = false;
                }
                else
                {
                    result.Data = response;
                }
            }
            catch(Exception ex)
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

    }
}
