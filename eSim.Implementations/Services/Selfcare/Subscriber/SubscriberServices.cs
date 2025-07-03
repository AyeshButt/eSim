using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Subscriber;

namespace eSim.Implementations.Services.Selfcare.Subscriber
{
   
    public class SubscriberServices : ISubscriber
    {
        private readonly IMiddlewareConsumeApi _consumeApi;
        public SubscriberServices(IMiddlewareConsumeApi consumeApi)
        {
            _consumeApi = consumeApi;
        }

        public async Task<Result<string>> ChangePasswordAsync(ChangePasswordDTORequest request)
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.ChangePassword;
            var result= await _consumeApi.Put<string,ChangePasswordDTORequest>(url, request);
            return result;


        }

        public async Task<Result<SubscriberDTO>> SubscriberDetailAsync()
        {
            var url = BusinessManager.MdwBaseURL + BusinessManager.SubscriberDetail;
            var result=await _consumeApi.Get<SubscriberDTO>(url);
            return result;

        }

        public async Task<Result<string>> UpdateSubscriberAsync(UpdateSubscriberDTORequest request)
        {
          var url= BusinessManager.MdwBaseURL+BusinessManager.Subscriberupdate;
            var result= await _consumeApi.Put<string, UpdateSubscriberDTORequest>(url,request);
            return result;
        }

        public Task<Result<string>> UploadProfileImage()
        {
            throw new NotImplementedException();
        }
    }
}
