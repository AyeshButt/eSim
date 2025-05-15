using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.Admin.Client
{
    public interface IClientSettings
    {
        public Task<Result<ClientSettingsDTO>> GetClientSettingsAsync(Guid id);
        public Task<Result<string>> UpdateClientSettingsAsync(ClientSettingsDTO input);
    }
}
