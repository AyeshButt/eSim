using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Admin.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Implementations.Services.Client
{
    public class ClientSettingsService : IClientSettings
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ClientSettingsService> _logger;

        public ClientSettingsService(ILogger<ClientSettingsService> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<Result<ClientSettingsDTO>> GetClientSettingsAsync(Guid id)
        {
            Result<ClientSettingsDTO> result = new();

            var clientSettings = await (from cs in _db.ClientSettings
                                        join c in _db.Client on cs.ClientId equals c.Id
                                        where cs.ClientId == id
                                        select new ClientSettingsDTO()
                                        {
                                            Id = cs.Id,
                                            ClientId = cs.ClientId,
                                            CreatedAt = cs.CreatedAt,
                                            CreatedBy = cs.CreatedBy,
                                            ModifiedAt = cs.ModifiedAt,
                                            FixedCommission = cs.FixedCommission,
                                            ModifiedBy = cs.ModifiedBy,
                                            Name = c.Name,
                                        }
                                  ).FirstOrDefaultAsync();

            if (clientSettings is null)
            {
                result.Data = null;
            }
            else
            {
                result.Data = clientSettings;
                result.Success = true;
            }

            return result;
        }

        public async Task<Result<string>> UpdateClientSettingsAsync(ClientSettingsDTO input)
        {
            Result<string> result = new();

            try
            {
                var findClientSettings = await _db.ClientSettings.FirstOrDefaultAsync(u => u.ClientId == input.ClientId);

                if (findClientSettings is null)
                {
                    var clientSettings = new ClientSettings()
                    {
                        Id = input.Id,
                        ClientId = input.ClientId,
                        CreatedAt = BusinessManager.GetDateTimeNow(),
                        ModifiedAt = BusinessManager.GetDateTimeNow(),
                        FixedCommission = input.FixedCommission,
                        ModifiedBy = input.ModifiedBy,
                        CreatedBy = input.CreatedBy,
                    };

                    await _db.ClientSettings.AddAsync(clientSettings);
                    await _db.SaveChangesAsync();

                    result.Success = true;

                    return result;
                }

                findClientSettings.FixedCommission = input.FixedCommission;
                findClientSettings.ModifiedBy = input.ModifiedBy;
                findClientSettings.ModifiedAt = BusinessManager.GetDateTimeNow();

                _db.ClientSettings.Update(findClientSettings);
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
