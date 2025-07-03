using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Subscriber;
using Microsoft.AspNetCore.Http;

namespace eSim.Implementations.Services.Selfcare.Subscriber
{
   
    public class SubscriberServices : ISubscriber
    {
        private readonly IMiddlewareConsumeApi _consumeApi;
        public SubscriberServices(IMiddlewareConsumeApi consumeApi)
        {
            _consumeApi = consumeApi;
        }

        public async Task<Result<string>> ChangePasswordAsync(ChangePasswordDTORequest request)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.ChangePassword;
            var result= await _consumeApi.Put<string,ChangePasswordDTORequest>(url, request);
            return result;


        }

        public async Task<Result<SubscriberDTO>> SubscriberDetailAsync()
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.SubscriberDetail;
            var result=await _consumeApi.Get<SubscriberDTO>(url);
            return result;

        }

        public async Task<Result<string>> UpdateSubscriberAsync(UpdateSubscriberDTORequest request)
        {
          var url= BusinessManager.MdwBaseURL+BusinessManager.Subscriberupdate;
            var result= await _consumeApi.Put<string, UpdateSubscriberDTORequest>(url,request);
            return result;
        }

        public async Task<Result<string>> UploadProfileImage(IFormFile file, string jwtToken)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.Subscriberupload;

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            using var form = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(ms.ToArray());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            form.Add(fileContent, "file", file.FileName); // "file" is key expected by API

            var response = await httpClient.PostAsync(url, form);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<string>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result!;
        }

        public async Task<Result<string>> UploadProfileImage(IFormFile file)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.Subscriberupload;
            return await _consumeApi.PostMultipartAsync<string>(url, file);
        }

    }
}
