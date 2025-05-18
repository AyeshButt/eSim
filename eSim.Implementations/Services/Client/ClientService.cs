using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Admin.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Implementations.Services.Client
{
    public class ClientService : IClient
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ClientService> _logger;

        public ClientService(ApplicationDbContext db, ILogger<ClientService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Result<string>> CreateClientAsync(ClientDTO input)
        {
            Result<string> result = new();

            try
            {
                var client = new EF.Entities.Client()
                {
                    Id = Guid.NewGuid(),
                    Name = input.Name,
                    Kid = BusinessManager.GenerateUniqueAlphanumericId(16),
                    IsActive = input.IsActive,
                    Secret = BusinessManager.GenerateUniqueAlphanumericId(20),
                    CreatedAt = BusinessManager.GetDateTimeNow(),
                    ModifiedAt = BusinessManager.GetDateTimeNow(),
                    CreatedBy = input.CreatedBy,
                    ModifiedBy = input.ModifiedBy,
                };

                await _db.Client.AddAsync(client);
                await _db.SaveChangesAsync();

                //var clientSettings = new ClientSettings
                //{
                //    Id= Guid.NewGuid(),
                //    ClientId = client.Id,
                //    FixedCommission = 0.00M,
                //};

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                result.Data = ex.Message;
            }
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
                    return result;
                }

                client.IsActive = client.IsActive ? false : true;

                await _db.SaveChangesAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

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
                };
            }

            return result;
        }

        public async Task<Result<string>> UpdateClientAsync(ClientDTO input)
        {
            Result<string> result = new();

            try
            {
                var findClient = await _db.Client.FindAsync(input.Id);

                if (findClient is null)
                {
                    return result;
                }

                findClient.IsActive = input.IsActive;
                findClient.Name = input.Name;
                findClient.ModifiedBy = input.ModifiedBy;
                findClient.ModifiedAt = BusinessManager.GetDateTimeNow();

                _db.Client.Update(findClient);
                await _db.SaveChangesAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                result.Data = ex.Message;
            }

            return result;
        }
    }
}
