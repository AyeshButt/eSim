using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;

using System.Threading.Tasks;
using Newtonsoft.Json;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.EF.Entities;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.DTOs.Global;
using Raven.Client.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace eSim.Common.StaticClasses
{
    public class ConsumeAPI : IConsumeApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ConsumeAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

                var statusCode = (int)request.StatusCode;

                //var jsonString = await request.Content.ReadAsStringAsync();

                response = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());



            }
            catch (Exception ex)
            {
                return default(T?);
            }
            return response;

        }
        //Consume Get Api by Ayesh
        public async Task<Result<T>?> GetApii<T>(string Url)
        {
            Result<T> response = new();
            T? result = default;
            HttpResponseMessage request = new();
            HttpRequestMessage req = new();

            try
            {
                req = new HttpRequestMessage(HttpMethod.Get, Url);
                req.Headers.Add("x-api-Key", _apiKey);

                request = await _httpClient.SendAsync(req);

                response.StatusCode = (int)request.StatusCode;

                if (request.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());
                    response.Data = result;
                    response.StatusCode = (int)request.StatusCode;

                }

                else
                {
                    response = JsonConvert.DeserializeObject<Result<T>>(await request.Content.ReadAsStringAsync());
                    response.Success = false;
                    response.StatusCode = (int)request.StatusCode;

                }

            }
            catch (Exception ex)
            {

                    response.Success = false;
                    response.Message = ex.Message;
                    response.Data = default;
                    response.StatusCode = (int)request.StatusCode;
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

                var jsonRequestPayload = JsonConvert.SerializeObject(input);

                var content = new StringContent(jsonRequestPayload, null, "application/json");

                httpreq.Content = content;

                var request = await _httpClient.SendAsync(httpreq);


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
