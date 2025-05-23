using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using eSim.Common.StaticClasses;
//using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Selfcare.Subscriber;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Selfcare.Authentication;
using eSim.Infrastructure.Interfaces.Selfcare.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web.Mvc;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices.JavaScript;
using eSim.Infrastructure.DTOs.Account;

namespace eSim.Implementations.Services.Selfcare.Authentication
{
    public class AuthenticationService(IHttpClientFactory httpClient, IMiddlewareConsumeApi consumeApi) : IAuthenticationService
    {
        private readonly HttpClient _httpClient = httpClient.CreateClient();
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;

        public async Task<string?> AuthenticateAsync(SignIn model)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.MidlewareLogin;
            var response = await _httpClient.PostAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode) 
            {

                var JsonReq = await response.Content.ReadAsStringAsync();
                var JsonResp = JsonConvert.DeserializeObject<LoginResponse>(JsonReq);

                //var Jsonstring = JsonSerializer.Deserialize<LoginResponse>(JsonReq);

                var token = JsonResp.access_token;

                return token;
            }

            return null;

        }



        public async Task<string?> Email(string Email)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.CheckEmail;

            var fullUrl = $"{url}?email={HttpUtility.UrlEncode(Email)}";

            var resp = await _httpClient.GetAsync(fullUrl);

            if (resp.IsSuccessStatusCode)
            { 
                var result = await resp.Content.ReadAsStringAsync();
                return result;
            }

            return null;
           
        }


        public async Task<Result<string?>> Create(SubscriberViewModel input)
        {
             Result<string?> reult = new();
            var url = BusinessManager.MdwBaseURL + BusinessManager.Subscriber;

            try
            {
                var resp = await _consumeApi.AuthPost<SubscriberViewModel, string>(url, input);

                if (resp != null)
                {
                    reult.Message = "Registered Succesfull";
                }

            }
            catch (Exception ex) 
            {
                reult.Success=false;
                reult.Message = "Something Went Wrong";
            }

            return reult;

        }

        public async Task<Result<string?>> ForgotPassword(ForgotPasswordDTO input)
        {
            Result<string?> result = new();

            var url = BusinessManager.MdwBaseURL + BusinessManager.forgotPass;

            try
            {
                var request = await _consumeApi.AuthPost<ForgotPasswordDTO, string>(url, input);

                if (request != null)
                {
                    result.Message = "Otp Sent to your Email";
                }

            }
            catch (Exception ex) { 
                result.Success=false;
                result.Message = "Something Went Wrong";
            }

            return result;
        }

    }
}
