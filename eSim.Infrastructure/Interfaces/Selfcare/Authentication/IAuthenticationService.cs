using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Selfcare.Subscriber;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Selfcare.Authentication;
using Microsoft.AspNetCore.Http;
using eSim.Infrastructure.DTOs.Account;

namespace eSim.Infrastructure.Interfaces.Selfcare.Authentication
{
    public interface IAuthenticationService
    {
       public Task<string?> AuthenticateAsync(SignIn model);

        public Task<Result<string?>> Create(SubscriberViewModel input);

        public Task<Result<string?>> ForgotPassword(ForgotPasswordDTO input);

        public Task<Result<string?>> OTPVarification(string input);

        public Task<Result<string?>> NewPassword(SubscriberResetPasswordDTO input);

        public Task<Result<string?>> Email(string Email);

    }
}
