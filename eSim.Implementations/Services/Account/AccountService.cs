using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs;
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

        public async Task<UserDTO?> VerifyEmail(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return null;
            }

            return new UserDTO()
            {
                Id = user.Id,
                Email = user.Email
            };
        }
        public async Task<Result> AddOTPDetails(OTPVerificationDTO details)
        {
            Result result = new Result();

            try
            {
                OTPVerification OTPDetails = new OTPVerification()
                {
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
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<OTPVerificationDTO?> GetValidOTPDetails(string userId)
        {
            var OTPDetails = await _db.OTPVerification.FirstOrDefaultAsync(u => u.UserId == userId && u.IsValid == true);

            return OTPDetails == null ? null : new OTPVerificationDTO
            {
                UserId = OTPDetails.UserId,
                OTP = OTPDetails.OTP,
                SentTime = OTPDetails.SentTime,
                Type = OTPDetails.Type,
                IsValid = OTPDetails.IsValid,
            };
        }
        public async Task<OTPVerificationDTO?> VerifyOTP(OTPVerificationDTO details)
        {
            var OTPDetails = await _db.OTPVerification.FirstOrDefaultAsync(u => u.OTP == details.OTP);

            return OTPDetails == null ? null : new OTPVerificationDTO
            {
                UserId = OTPDetails.UserId,
                OTP = OTPDetails.OTP,
                SentTime = OTPDetails.SentTime,
                Type = OTPDetails.Type,
                IsValid = OTPDetails.IsValid,
            };
        }
        public async Task<Result> RemoveOTPDetails(string userId)
        {
            Result result = new();
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
               result.Message = ex.Message;
            }
            return result;
        }

    }
}
