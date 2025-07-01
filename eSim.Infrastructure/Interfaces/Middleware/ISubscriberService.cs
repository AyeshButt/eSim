using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface ISubscriberService
    {
        Task<Result<string>> SubscriberEmailExists(string email);
        Task<Result<string>> CreateSubscriber(SubscriberDTORequest input);

        Task<Result<string>> UpdateSubscriberAsync(Guid id, UpdateSubscriberDTORequest request);
        Task<Result<string?>> UploadProfileImageAsync(IFormFile file, ProfileImageDTORequest dto);


        public Task<IQueryable<SubscriberDTO>> GetClient_SubscribersListAsync(string id);
        public Task<IQueryable<SubscriberDTO>> GetSubscribersListAsync();
        Task<Result<SubscriberDTO>> GetSubscriberDetailAsync(Guid subscriberId);


    }
}
