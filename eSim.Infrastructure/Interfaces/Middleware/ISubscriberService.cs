using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface ISubscriberService
    {
        Task<bool> EmailExists(string email);
        Task<Result<string>> CreateSubscriber(SubscriberRequestDTO input);
    }
}
