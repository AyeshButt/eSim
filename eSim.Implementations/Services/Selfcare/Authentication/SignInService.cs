using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Selfcare.Authentication;
using eSim.Infrastructure.Interfaces.Selfcare.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace eSim.Implementations.Services.Selfcare.Authentication
{
    public class SignInService : ISignIn
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseURL = "https://localhost:7264/Auth/login";
        private readonly string TokenKey = "API-Token";

        public SignInService( IHttpClientFactory httpClient,  IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        


        }

        public async Task<bool> AuthenticateAsync(SignIn model)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseURL, model);

            if (response.IsSuccessStatusCode) 
            {
                var token = await response.Content.ReadAsStringAsync();
                _httpContextAccessor.HttpContext.Session.SetString(TokenKey, token); 
            }
            return true;


        }
        public bool IsAuthenticated()
        {
            var token = !string.IsNullOrEmpty(GetToken());
            
            return token;
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext.Session.GetString(TokenKey);
        }

    }
}
