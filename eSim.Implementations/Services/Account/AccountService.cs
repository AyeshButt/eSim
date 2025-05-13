using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.AccessControl;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Admin.Account;
using Microsoft.EntityFrameworkCore;

using static System.Net.WebRequestMethods;

namespace eSim.Implementations.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;

        public AccountService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Result<UserDTO>> VerifyEmail(string email)
        {
            Result<UserDTO> result = new();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
            {
                result.Data = null;
            }
            else
            {
                var model = new UserDTO()
                {
                    Id = user.Id,
                    Email = user.Email
                };

                result.Success = true;
                result.Data = model;
            }

            return result;
        }
        public async Task<Result<string>> AddOTPDetails(OTPVerificationDTO details)
        {
            Result<string> result = new();

            try
            {
                OTPVerification OTPDetails = new OTPVerification()
                {
                    Id = Guid.NewGuid().ToString(),
                    IsValid = details.IsValid,
                    OTP = details.OTP,
                    SentTime = details.SentTime,
                    Type = details.Type,
                    UserId = details.UserId ?? string.Empty,
                };

                await _db.OTPVerification.AddAsync(OTPDetails);
                await _db.SaveChangesAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Data = ex.Message;
            }

            return result;
        }

        public async Task<Result<OTPVerificationDTO>> GetValidOTPDetails(string userId)
        {
            Result<OTPVerificationDTO> result = new();

            var OTPDetails = await _db.OTPVerification.FirstOrDefaultAsync(u => u.UserId == userId && u.IsValid == true);

            if (OTPDetails == null)
            {
                result.Data = null;
            }
            else
            {
                result.Data = new OTPVerificationDTO
                {
                    UserId = OTPDetails.UserId,
                    OTP = OTPDetails.OTP,
                    SentTime = OTPDetails.SentTime,
                    Type = OTPDetails.Type,
                    IsValid = OTPDetails.IsValid,
                };

                result.Success = true;
            }

            return result;
        }
        public async Task<Result<OTPVerificationDTO>> VerifyOTP(OTPVerificationDTO input)
        {
            Result<OTPVerificationDTO> result = new();

            var OTPDetails = await _db.OTPVerification.FirstOrDefaultAsync(u => u.OTP == input.OTP);
           
            if (OTPDetails == null)
            {
                result.Data = null;
            }
            else
            {
                result.Data = new OTPVerificationDTO
                {
                    UserId = OTPDetails.UserId,
                    OTP = OTPDetails.OTP,
                    SentTime = OTPDetails.SentTime,
                    Type = OTPDetails.Type,
                    IsValid = OTPDetails.IsValid,
                };

                result.Success = true;
            }
            return result;

        }
        public async Task<Result<string>> RemoveOTPDetails(string userId)
        {
            Result<string> result = new();
            try
            {
                var otpDetailsList = await _db.OTPVerification.Where(u => u.UserId == userId).ToListAsync();

                if (otpDetailsList.Any())
                {
                    _db.OTPVerification.RemoveRange(otpDetailsList);
                    await _db.SaveChangesAsync();
                    result.Success = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Data = ex.Message;
            }
            return result;
        }

    }
}
