using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface ISubscriberService
    {
        Task<Result<string>> EmailExists(string email);
        Task<Result<string>> CreateSubscriber(SubscriberRequestDTO input);

        Task<Result<string>> UpdateSubscriberAsync(Guid id, UpdateSubscriberDTO input);
        Task<Result<string?>> UploadProfileImageAsync(IFormFile file, ProfileImageDTO dto);




    }
}
