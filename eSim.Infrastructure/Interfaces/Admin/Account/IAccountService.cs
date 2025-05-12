using eSim.Infrastructure.DTOs;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.Admin.Account
{
    public interface IAccountService
    {
        public Task<UserDTO?> VerifyEmail(string email);
        public Task<Result> AddOTPDetails(OTPVerificationDTO details);
        public Task<OTPVerificationDTO?> GetValidOTPDetails(string userId);
        public Task<OTPVerificationDTO?> VerifyOTP(OTPVerificationDTO details);
        public Task<Result> RemoveOTPDetails(string userId);

    }
}
