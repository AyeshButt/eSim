using eSim.Infrastructure.DTOs.Client;
using eSim.Infrastructure.DTOs.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.Interfaces.Admin.Client
{
    public interface IClient
    {
        public Task<Result<ClientDTO>> GetClientAsync(Guid id);
        public Task<IQueryable<ClientDTO>> GetAllClientsAsync();
        public Task<Result<string>> CreateClientAsync(ClientDTO input);
        public Task<Result<string>> UpdateClientAsync(ClientDTO input);
        public Task<Result<string>> DisableClientAsync(Guid id);

    }
}
