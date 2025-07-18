﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
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

        public string? Authenticate(AuthDTORequest input)
        {
            Subscribers? subscriber = _db.Subscribers.FirstOrDefault(a => a.Active && a.Email == input.Email);

            if (subscriber is null || !PasswordHasher.VerifyPassword(input.Password, subscriber.Hash))
                return null;

            var token = JWTConfiguration(subscriber.Id.ToString());

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //6. move jwt code in a private function - made by ayesh

        private JwtSecurityToken JWTConfiguration(string id)
        {
            var claims = new[]
            {
                 new Claim(BusinessManager.SubscriberId, id)
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

            return token;
        }
    }
}
