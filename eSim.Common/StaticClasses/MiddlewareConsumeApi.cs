using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.QRDownload;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace eSim.Common.StaticClasses
{
    public class MiddlewareConsumeApi(HttpClient http, IHttpContextAccessor httpContext) : IMiddlewareConsumeApi
    {
        private readonly HttpClient _http = http;
        private readonly IHttpContextAccessor _httpContext = httpContext;

        #region QR code consumption
        public async Task<FileDownloadResult> DownloadQrCodeAsync(string url)
        {
            var result = new FileDownloadResult();

            try
            {
                var token = _httpContext.HttpContext?.User?.FindFirst("Token")?.Value;
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    result.Message = "QR code download failed.";
                    result.Success = false;
                    return result;
                }

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.ToString();
                var contentDisposition = response.Content.Headers.ContentDisposition?.FileName ??
                                         response.Content.Headers.ContentDisposition?.FileNameStar;

                // Extract filename manually if not found
                if (string.IsNullOrEmpty(contentDisposition) &&
                    response.Content.Headers.Contains("Content-Disposition"))
                {
                    var disposition = response.Content.Headers.GetValues("Content-Disposition").FirstOrDefault();
                    if (disposition != null && disposition.Contains("filename="))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(disposition, "filename[^;=]*=['\"]?([^'\"]+)");
                        contentDisposition = match.Success ? match.Groups[1].Value : "download.png";
                    }
                }

                result.FileBytes = fileBytes;
                result.ContentType = contentType ?? "application/octet-stream";
                result.FileName = contentDisposition ?? "download.png";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = "An error occurred while downloading the QR code. " + ex.Message;
                result.Success = false;
            }

            return result;
        }

        #endregion

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
                response = JsonConvert.DeserializeObject<Result<T?>>(content);

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

                Console.WriteLine("Bundle Detail" + await jsonResponse.Content.ReadAsStringAsync());

                response = JsonConvert.DeserializeObject<Result<T>>(await jsonResponse.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                return new Result<T?>() { Message = "Unable to Create", Success = false };
            }

            return response;
        }

        #endregion

        #region put
        public async  Task<Result<T?>> Put<T, I>(string url, I? input)
        {
            Result<T?> response = new();

            try
            {
                var token = _httpContext.HttpContext?.User?.FindFirst("Token")?.Value;

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var jsonResponse = await _http.PatchAsJsonAsync(url, input);

                Console.WriteLine("Bundle Detail: " + await jsonResponse.Content.ReadAsStringAsync());

                response = JsonConvert.DeserializeObject<Result<T>>(await jsonResponse.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new Result<T?>() { Message = "Unable to Update", Success = false };
            }

            return response;
        }
        #endregion
    }
}
    