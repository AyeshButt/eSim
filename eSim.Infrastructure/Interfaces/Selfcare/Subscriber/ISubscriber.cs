﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;
using Microsoft.AspNetCore.Http;

namespace eSim.Infrastructure.Interfaces.Selfcare.Subscriber
{  
    public interface ISubscriber
    {
        Task<Result<SubscriberDTO>> SubscriberDetailAsync();
        Task<Result<string>> UpdateSubscriberAsync(UpdateSubscriberDTORequest request);
        Task<Result<string>> ChangePasswordAsync(ChangePasswordDTORequest request);
        Task<Result<string>> UploadProfileImage(IFormFile file);

        




    }
}
