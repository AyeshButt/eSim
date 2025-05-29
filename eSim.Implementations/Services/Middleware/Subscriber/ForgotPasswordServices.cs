using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
//using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace eSim.Implementations.Services.Middleware.Subscriber
{  
    public class ForgotPasswordServices : IForgotPassword
    {
        private readonly ApplicationDbContext _db;
      //  private readonly IEmailService _emailService;
        public ForgotPasswordServices(ApplicationDbContext db/* IEmailService emailService*/)
        {
            _db = db;
           // _emailService = emailService;
        }
        #region ForgotPassword
        public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordDTO input)
        {
            var result = new Result<string>();

            var user = await _db.Subscribers.FirstOrDefaultAsync(u => u.Email == input.Email);

            if (user == null)
            {
                result.Success = false;
                result.Message = "Email not found.";
                result.Data = null;
                return result;
            }

            var otp = new Random().Next(100000, 999999).ToString();

            var otpRecord = new OTPVerification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id.ToString(),
                OTP = otp,
                SentTime = DateTime.UtcNow,
                IsValid = true,
                Type = "ForgotPassword"
            };

            _db.OTPVerification.Add(otpRecord);
            await _db.SaveChangesAsync();

            //var emailResult =  _emailService.SendEmail(new EmailDTO
            //{
            //    To = user.Email,
            //    Subject = "Your OTP for Password Reset",
            //    Body = $"Your OTP is: {otp}."
            //});

            //if (!emailResult.Success)
            //{
            //    result.Success = false;
            //    result.Message = "Failed to send OTP email.";
            //    result.Data = emailResult.Data;
            //    return result;
            //}

            result.Success = true;
            result.Message = "OTP sent to your email.";
            result.Data = null;
            return result;
        }

        #endregion

        #region VerifyOtp
        public async Task<Result<string>> VerifyOtpAsync(string otp)
        {
            var result = new Result<string>();

            var otpRecords = await _db.OTPVerification

                .Where(o => o.OTP == otp && o.IsValid && o.Type == "ForgotPassword")
                .ToListAsync();

            var otpRecord = otpRecords
                .FirstOrDefault(o => (DateTime.UtcNow - o.SentTime).TotalMinutes <= 10);

            if (otpRecord == null)
            {
                result.Success = false;
                result.Message = "Invalid or expired OTP.";
         
                return result;
            }

            otpRecord.IsValid = false;
            await _db.SaveChangesAsync();

            result.Success = true;
            result.Message = "OTP verified successfully.";
   
  
            return result;
        }

        #endregion

        #region ResetPassword

        public async Task<Result<string>> ResetPasswordAsync(SubscriberResetPasswordDTO input)
        {
            var result = new Result<string>();

            var user = await _db.Subscribers.FirstOrDefaultAsync(x => x.Email == input.Email);
            if (user == null)
            {
                result.Success = false;
                result.Message = "User not found.";
                return result;
            }
           
           user.Hash = PasswordHasher.HashPassword(input.NewPassword);
            user.ModifiedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            //var emailResult =  _emailService.SendEmail(new EmailDTO
            //{
            //    To = user.Email,
            //    Subject = "Password Changed Successfully",
            //    Body = "Your password has been changed successfully."
            //});

            //if (!emailResult.Success)
            //{
            //    result.Success = false;
            //    result.Message = "Password changed but failed to send confirmation email.";
            //    return result;
            //}

            result.Success = true;
            result.Message = "Password changed successfully.";
            return result;
        }


      
        #endregion

        #region changePassowrd
        public async Task<Result<string>> ChangePasswordAsync(ChangePasswordDTO input)
        {
            var result = new Result<string>();


            var user = await _db.Subscribers.FirstOrDefaultAsync(u => u.Email == input.Email);
            if (user == null)
            {
                result.Success = false;
                result.Message = "User not found.";
           
                return result;
            }

            // Step 2: Verify old password
            var oldHash = PasswordHasher.HashPassword(input.OldPassword);
            if (user.Hash != oldHash)
            {
                result.Success = false;
                result.Message = "Old password is incorrect.";
          
                return result;
            }

            // Step 3: Check new & confirm password match
            if (input.NewPassword != input.ConfirmPassword)
            {
                result.Success = false;
                result.Message = "New password and Confirm password do not match.";
         
                return result;
            }

            // Step 4: Proceed with reset
            var resetDto = new SubscriberResetPasswordDTO
            {
                Email= input.Email,
                NewPassword = input.NewPassword,
                ConfirmPassword = input.ConfirmPassword
            };

            var resetResult = await ResetPasswordAsync(resetDto);
            return resetResult;
        }





        #endregion
    }

}
