using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface IForgotPassword
    {
        Task<Result<string>> ForgotPasswordAsync(ForgotPasswordDTORequest input);
        Task<Result<string>> VerifyOTPAsync(string otp);
        Task<Result<string>> ResetPasswordAsync(Guid logged, SubscriberResetPasswordDTO input);
        Task<Result<string>> ChangePasswordAsync(Guid logged,ChangePasswordDTORequest input);
 




    }
}
