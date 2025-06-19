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
using System.Net.Http.Headers;
using Org.BouncyCastle.Utilities;

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
            HttpRequestMessage req = new();
            HttpResponseMessage request = new();

            T? result = default;
            Result<T> response = new();

            try
            {
                req = new HttpRequestMessage(HttpMethod.Get, Url);
                req.Headers.Add("x-api-Key", _apiKey);

                request = await _httpClient.SendAsync(req);

                response.StatusCode = (int)request.StatusCode;

                if (request.IsSuccessStatusCode)
                {
                    #region checking content type for qr code
                    if (request.Content.Headers.ContentType?.MediaType == BusinessManager.ImageMediaContentType)
                    {
                        var imageBytes = await request.Content.ReadAsByteArrayAsync();

                        #region typecast imageBytes to generic result variable
                        if (typeof(T) == typeof(byte[]))
                        {
                            result = (T)(object)imageBytes; // Cast through object to match T
                        }
                        else
                        {
                            response.Success = false;
                            response.StatusCode = StatusCodes.Status500InternalServerError;
                            response.Message = string.Empty;

                            return response;
                        }
                        #endregion

                    }
                    #endregion

                    else
                    {
                        result = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());
                    }

                    response.Data = result;

                }

                else
                {
                    response = JsonConvert.DeserializeObject<Result<T>>(await request.Content.ReadAsStringAsync());
                    response.Success = false;
                }
                response.StatusCode = (int)request.StatusCode;

            }
            #region typecast exception handling
            catch (InvalidCastException ex) when (ex is InvalidCastException)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            #endregion
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = default;
                response.StatusCode = (int)request.StatusCode;
            }

            return response;
        }
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

                int statusCode = (int)request.StatusCode;

                response = JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());

            }
            catch (Exception e)
            {
                return default(T?);
            }
            return response;
        }
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
