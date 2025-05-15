using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.Interfaces.Middleware;

namespace eSim.Implementations.Services.Middleware
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string? GetOrders()
        {
            var url = "https://api.esim-go.com/v2.4/catalogue?perPage=50&direction=desc&orderBy=speed&region=Asia";
            var request = new HttpRequestMessage(HttpMethod.Get, url);


            


            // ✅ Add API Key in the header
            request.Headers.Add("x-api-key", "5iiPSVJSr0LUtbJRLxHyxOVNg-kyYZ4UngIGktEs");

            var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine("API Error: " + error);
                return null;
            }

            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
