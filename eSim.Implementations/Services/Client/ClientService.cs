using eSim.Common.Enums;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Admin.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Raven.Client.Linq.LinqPathProvider;

namespace eSim.Implementations.Services.Client
{
    public class ClientService : IClient
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ClientService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;


        public ClientService(ApplicationDbContext db, ILogger<ClientService> logger, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration config)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<Result<ClientUserDTO>> CreateClientAsync(ClientDTO input)
        {
            Result<ClientUserDTO> result = new();

            var roleId = _config.GetValue<string>("RoleConfiguration:roleid") ?? string.Empty;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                result.Success = false;
                result.Message = "Role not found.";
                return result;
            }

            #region client and user creation logic

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                #region client mapping
                var client = new EF.Entities.Client()
                {
                    Id = Guid.NewGuid(),
                    Name = input.Name,
                    PrimaryEmail = input.PrimaryEmail,
                    Kid = BusinessManager.GenerateUniqueAlphanumericId(16),
                    IsActive = input.IsActive,
                    Secret = BusinessManager.GenerateUniqueAlphanumericId(20),
                    CreatedAt = BusinessManager.GetDateTimeNow(),
                    ModifiedAt = BusinessManager.GetDateTimeNow(),
                    CreatedBy = input.CreatedBy,
                    ModifiedBy = input.ModifiedBy,
                };
                #endregion

                #region Application User mapping
                var clientUser = new ApplicationUser()
                {
                    UserName = client.PrimaryEmail,
                    Email = client.PrimaryEmail,
                    UserType = (int)AspNetUsersTypeEnum.Client,
                    UserRoleId = roleId,
                };
                #endregion

                #region Password generation and user creation
                var generatePassword = BusinessManager.GenerateUniqueAlphanumericId(16);

                var userCreationResult = await _userManager.CreateAsync(clientUser, generatePassword);

                #endregion

                if (!userCreationResult.Succeeded)
                {
                    result.Success = false;
                    return result;
                }

                #region Adding user to the role - Add claims from role to user

                await _userManager.AddToRoleAsync(clientUser, role.Name);

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                if (roleClaims.Any())
                {
                    await _userManager.AddClaimsAsync(clientUser, roleClaims);
                }

                #endregion


                await _db.Client.AddAsync(client);
                await _db.SaveChangesAsync();

                result.Success = true;

                await transaction.CommitAsync();


                var model = new ClientUserDTO()
                {
                    Password = generatePassword,
                    UserId = clientUser.Id,
                    User = clientUser
                };

                result.Data = model;


            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(ex.Message);

                result.Data = null;

                result.Success = false;
            }
            #endregion

            return result;
        }

        public async Task<Result<string>> DisableClientAsync(Guid id)
        {
            Result<string> result = new();

            try
            {
                var client = await _db.Client.FindAsync(id);

                if (client is null)
                {
                    result.Data = null;
                    result.Success = false;
                    return result;
                }

                client.IsActive = client.IsActive ? false : true;

                await _db.SaveChangesAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                result.Success = false;
                result.Data = ex.Message;
            }

            return result;
        }

        public async Task<IQueryable<ClientDTO>> GetAllClientsAsync()
        {

            var clientList = _db.Client.AsNoTracking().Select(u => new ClientDTO()
            {
                Id = u.Id,
                IsActive = u.IsActive,
                Kid = u.Kid,
                Name = u.Name,
                Secret = u.Secret,
                PrimaryEmail = u.PrimaryEmail,
            }).AsQueryable();

            return await Task.FromResult(clientList);
        }

        public async Task<Result<ClientDTO>> GetClientAsync(Guid id)
        {
            Result<ClientDTO> result = new();

            var client = await _db.Client.FindAsync(id);

            if (client is null)
            {
                result.Data = null;
                result.Success = false;
            }
            else
            {
                result.Success = true;

                result.Data = new ClientDTO()
                {
                    Id = client.Id,
                    IsActive = client.IsActive,
                    Kid = client.Kid,
                    Name = client.Name,
                    Secret = client.Secret,
                    PrimaryEmail = client.PrimaryEmail,
                };
            }

            return result;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, Guid id)
        {
            email = email.Trim();

            // New record
            if (id == Guid.Empty)
            {
                return !await _db.Client.AnyAsync(c => c.PrimaryEmail == email);
            }

            // Existing record (update), exclude this Id
            return !await _db.Client.AnyAsync(c => c.PrimaryEmail == email && c.Id != id);
        }

        public async Task<bool> IsNameUniqueAsync(string name, Guid id)
        {
            name = name.Trim();

            if (id == Guid.Empty)
            {
                return !await _db.Client.AnyAsync(c => c.Name == name);
            }

            return !await _db.Client.AnyAsync(c => c.Name == name && c.Id != id);
        }

        public async Task<Result<string>> UpdateClientAsync(ClientDTO input)
        {
            Result<string> result = new();

            try
            {
                var findClient = await _db.Client.FindAsync(input.Id);

                if (findClient is null)
                {
                    result.Success = false;

                    return result;
                }

                findClient.Name = input.Name;
                findClient.ModifiedBy = input.ModifiedBy;
                findClient.ModifiedAt = BusinessManager.GetDateTimeNow();
                findClient.PrimaryEmail = input.PrimaryEmail;

                _db.Client.Update(findClient);
                await _db.SaveChangesAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                result.Success = false;
                result.Data = ex.Message;
            }

            return result;
        }
    }
}
