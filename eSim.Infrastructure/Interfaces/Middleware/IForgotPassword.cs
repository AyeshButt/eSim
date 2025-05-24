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
        Task<Result<string>> ForgotPasswordAsync(ForgotPasswordDTO input);
        Task<Result<string>> VerifyOtpAsync(string otp);
        Task<Result<string>> ResetPasswordAsync(SubscriberResetPasswordDTO input);
        Task<Result<string>> ChangePasswordAsync(ChangePasswordDTO input);




    }
}
