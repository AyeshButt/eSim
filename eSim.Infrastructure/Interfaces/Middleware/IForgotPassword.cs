using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface IForgotPassword
    {
        Task<Result<string>> ForgotPasswordAsync(ForgotPasswordDTORequest input);
        Task<Result<string>> VerifyOTPAsync(string otp);
        Task<Result<string>> ResetPasswordAsync(SubscriberResetPasswordDTORequest input);
        Task<Result<string>> ChangePasswordAsync(ChangePasswordDTORequest input);




    }
}
