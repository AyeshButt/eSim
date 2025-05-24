using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<T?> AuthPost<T, I>(string url, T? input)
        {
            T? response = default;

            if (input == null)
            {
                return response;
            }

            try
            {
                var request = await _http.PostAsJsonAsync(url, input);
                if (request.IsSuccessStatusCode)
                { 
                    response = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());
                }

            }
            catch (Exception e)
            {
                return default(T?);
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
