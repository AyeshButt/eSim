using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace eSim.Common.StaticClasses
{
    public class MiddlewareConsumeApi(HttpClient http, IHttpContextAccessor httpContext) : IMiddlewareConsumeApi
    {
        private readonly HttpClient _http = http;
        private readonly IHttpContextAccessor _httpContext = httpContext;


        #region Get Request
        
        /// <summary>
        /// Get Api Consumption for any type of get requests
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>

        public async Task<Result<T?>> Get<T>(string url)
        {
            Result<T?> response = default;

            try
            {
                var token = _httpContext.HttpContext?.User?.FindFirst("Token")?.Value;

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = await _http.GetAsync(url);

                var content = await request.Content.ReadAsStringAsync();

                
                if (request.IsSuccessStatusCode) 
                {
                    response = JsonConvert.DeserializeObject<Result<T?>>(content);
                }

                else if (string.IsNullOrWhiteSpace(content))
                {
                    return new Result<T?> { Message = "Empty response from server", Success = false };
                }


            }
            catch (Exception ex)
            {
                return new Result<T?>() { Message = "Unable to fetch", Success = false };
            }

            return response;
        }


        #endregion

        #region Post Auth Api 

        /// <summary>
        /// Generic post api call for only auth ost request!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Result<T?>> Post<T, I>(string url, I input)
        {
            Result<T?> response = new();

            try
            {
                var token = _httpContext.HttpContext?.User?.FindFirst("Token")?.Value;

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var jsonResponse  = await _http.PostAsJsonAsync(url, input);
                Console.WriteLine("response" + await jsonResponse.Content.ReadAsStringAsync());
                response = JsonConvert.DeserializeObject<Result<T>>(await jsonResponse.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                return new Result<T?>() { Message = "Unable to Create", Success = false };
            }

            return response;
        }
        #endregion
    }
}
