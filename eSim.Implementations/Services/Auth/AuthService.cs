using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eSim.Implementations.Services.Auth
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public string? Authenticate(string username, string password)
        {
            // Replace with DB check in real apps
            if (username != "admin" || password != "123")
                return null;

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username)
        };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
                    signingCredentials: creds
                );

            

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
