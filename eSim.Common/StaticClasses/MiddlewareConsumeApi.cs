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

        public async Task<T?> Get<T>(string url)
        {
            T? response = default;

            try
            {
                var token = _httpContext.HttpContext?.User?.FindFirst("Token")?.Value;

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = await _http.GetAsync(url);


                if (request.IsSuccessStatusCode) 
                {
                    var content = await request.Content.ReadAsStringAsync();

                    response = JsonConvert.DeserializeObject<T>(content);
                }

                

            }
            catch (Exception ex)
            {
                return default(T?);
            }
            return response;
        }

        #endregion

        #region Post Auth Api 
        public async Task<Result<T?>> AuthPost<T, I>(string url, T? input)
        {
            Result<T?> response = default;

            if (input == null)
            {
                return response;
            }

            try
            {
                var jsonResponse  = await _http.PostAsJsonAsync(url, input);

                var request = JsonConvert.DeserializeObject<Result<T>>(await jsonResponse.Content.ReadAsStringAsync());

                if (jsonResponse.IsSuccessStatusCode)
                {
                    if(response != null)

                        response.Data = request.Data;
                        response.Message = request.Message;
                }

                response.Success = request.Success;
                response.Message = request.Message; 

            }
            catch (Exception e)
            {
                return default(Result<T?>);
            }
            return response;
        }
        #endregion

        public Task<T?> PostApi<T, I>(string url, I data)
        {
            throw new NotImplementedException();
        }

        public Task<T?> PutApi<T, I>(string url, I data)
        {
            throw new NotImplementedException();
        }
    }
}
