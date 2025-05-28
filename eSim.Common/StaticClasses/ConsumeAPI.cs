using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using eSim.Infrastructure.Interfaces.ConsumeApi;

namespace eSim.Common.StaticClasses
{
    public class ConsumeAPI : IConsumeApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ConsumeAPI( HttpClient httpClient)
        {
            _httpClient = new HttpClient();
            _apiKey = "5iiPSVJSr0LUtbJRLxHyxOVNg-kyYZ4UngIGktEs";
        }

        //Consume Get Api by Bilal
        public async Task<T?> GetApi<T>(string Url)
        {
            T? response = default;  

            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Url);
                req.Headers.Add("x-api-Key", _apiKey);

                var request = await _httpClient.SendAsync(req);

                response = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                return default(T?);
            }
            return response;
     
}

        //Consume Post Api
        public async Task<T?> PostApi<T, I>(string Url, I? input)
        {
            T? response = default;

            if (input == null)
            {
                return response;
            }

            try
            {
                var httpreq = new HttpRequestMessage(HttpMethod.Post, Url);
                httpreq.Headers.Add("X-API-Key", _apiKey);
                var request = await _httpClient.PostAsJsonAsync(Url, input);
               

                response = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return default(T?);
            }
            return response;
        }

        //Consume Put Api

        public async Task<T?> PutApi<T, I>(string Url, I input)
        {
            T? response = default;

            if (input == null)
            {
                return response;
            }

            try
            {
                var request = await _httpClient.PutAsJsonAsync(Url, input);
                request.Headers.Add("x-apiKey", _apiKey);

                response = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {

                return default(T?);
            }
            return response;
        }
    }
}
