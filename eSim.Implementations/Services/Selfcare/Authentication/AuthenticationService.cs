using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Account;
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
using Microsoft.AspNetCore.Http.HttpResults;

namespace eSim.Implementations.Services.Selfcare.Authentication
{
    public class AuthenticationService(IHttpClientFactory httpClient, IMiddlewareConsumeApi consumeApi) : IAuthenticationService
    {
        private readonly HttpClient _httpClient = httpClient.CreateClient();
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;


        #region signIn 
        public async Task<string?> AuthenticateAsync(SignIn model)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.MidlewareLogin;
            var response = await _httpClient.PostAsJsonAsync(url, model);

            if (response.IsSuccessStatusCode) 
            {

                var JsonReq = await response.Content.ReadAsStringAsync();

                var JsonResp = JsonConvert.DeserializeObject<LoginResponse>(JsonReq);

                var token = JsonResp.access_token;

                return token;
            }

            return null;

        }

        #endregion

        #region Email Varification

        public async Task<Result<string?>> Email(string Email)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.CheckEmail;

            var fullUrl = $"{url}?email={HttpUtility.UrlEncode(Email)}";

            var resp = await _consumeApi.Get<string>(fullUrl);

            return resp;

        }


        #endregion

        #region Subscriber registration

        public async Task<Result<string?>> Create(SubscriberViewModel input)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.Subscriber;


            var resp = await _consumeApi.Post<string, SubscriberViewModel>(url, input);

            return resp;

        }

        #endregion

        #region Forgot Password

        public async Task<Result<string?>> ForgotPassword(ForgotPasswordDTO input)
        {

            var url = BusinessManager.MdwBaseURL + BusinessManager.forgotPass;

            var request = await _consumeApi.Post<string, ForgotPasswordDTO>(url, input);

            return request;
        }

        #endregion

        #region otp varification

        public async Task<Result<string?>> OTPVarification(string input)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.OTP;

            var request = await _consumeApi.Post<string, string>(url, input);

            return request;
        }

        #endregion

        #region new password

        public async Task<Result<string?>> NewPassword(SubscriberResetPasswordDTO input)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.resetPass;

            var request = await _consumeApi.Post<string, SubscriberResetPasswordDTO>(url, input);

            return request;
        }
        #endregion
    }
}
