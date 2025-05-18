using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Middleware;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eSim.Implementations.Services.Auth
{
    public class AuthService : IAuthService
    {



        private readonly ApplicationDbContext _db;

        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public string? Authenticate(AuthDTO input)
        {


            var client = _db.Client.FirstOrDefault(a => a.IsActive && a.Kid == input.Username && a.Secret == input.Password);

            if (client is null)
                return null;

            var claims = new[]
            {
                 new Claim("client-id", client.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );



            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
