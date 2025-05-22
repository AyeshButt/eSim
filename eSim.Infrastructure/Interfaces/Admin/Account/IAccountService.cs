using eSim.Infrastructure.DTOs.AccessControl;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.Admin.Account
{
    public interface IAccountService
    {
        public Task<Result<UserDTO>> VerifyEmail(string email);
        public IQueryable<AspNetUsersTypeDTO> GetUsersType();
        public Task<Result<string>> AddOTPDetails(OTPVerificationDTO input);
        public Task<Result<OTPVerificationDTO>> GetValidOTPDetails(string userId);
        public Task<Result<OTPVerificationDTO>> VerifyOTP(OTPVerificationDTO input);
        public Task<Result<string>> RemoveOTPDetails(string userId);

    }
}
