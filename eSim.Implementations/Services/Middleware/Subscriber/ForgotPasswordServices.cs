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
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using eSim.Common.Enums;
using System.ServiceModel.Channels;

namespace eSim.Implementations.Services.Middleware.Subscriber
{
    public class ForgotPasswordServices : IForgotPassword
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailService _email;
        public ForgotPasswordServices(ApplicationDbContext db, IEmailService email)
        {
            _db = db;
            _email = email;
        }
        #region ForgotPassword
        public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordDTORequest input)
        {
            var result = new Result<string>();
            try
            {
                var subscriber = await _db.Subscribers.FirstOrDefaultAsync(u => u.Email == input.Email);

                if (subscriber == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.SubscriberNotFound;
                    return result;
                }

                var otpDetails = new OTPVerification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = subscriber.Id.ToString(),
                    OTP = new Random().Next(100000, 999999).ToString(),
                    SentTime = BusinessManager.GetDateTimeNow(),
                    IsValid = true,
                    Type = OTPTypeEnum.ForgotPassword.ToString(),
                };

                await _db.OTPVerification.AddAsync(otpDetails);
                await _db.SaveChangesAsync();

                try
                {
                    var emailResult = _email.SendEmail(new EmailDTO
                    {
                        To = subscriber.Email,
                        Subject = BusinessManager.OTPSubject,
                        Body = BusinessManager.GetOTPBody(otpDetails.OTP)
                    });
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    result.Message = BusinessManager.OTPSendSuccessfully;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion

        #region VerifyOtp
        public async Task<Result<string>> VerifyOTPAsync(string otp)
        {
            var result = new Result<string>();

            if (string.IsNullOrEmpty(otp))
            {
                result.Success = false;
                result.Message = BusinessManager.RequiredOTP;
                return result;
            }

            try
            {

                var otpRecords = await _db.OTPVerification

                .Where(o => o.OTP == otp && o.IsValid == true && o.Type == OTPTypeEnum.ForgotPassword.ToString())
                .ToListAsync();

                var otpExpireTime = otpRecords
                    .FirstOrDefault(o => (DateTime.UtcNow - o.SentTime).TotalMinutes <= 10);

                if (otpExpireTime is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.InvalidOTP;

                    return result;
                }

                otpExpireTime.IsValid = false;
                await _db.SaveChangesAsync();


                result.Message = BusinessManager.OTPVerified;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion

        #region ResetPassword

        public async Task<Result<string>> ResetPasswordAsync(SubscriberResetPasswordDTORequest input)
        {
            var result = new Result<string>();

            try
            {
                var subscriber = await _db.Subscribers.FirstOrDefaultAsync(x => x.Email == input.Email);

                if (subscriber == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.SubscriberNotFound;

                    return result;
                }

                subscriber.Hash = PasswordHasher.HashPassword(input.NewPassword);
                subscriber.ModifiedAt = BusinessManager.GetDateTimeNow();

                await _db.SaveChangesAsync();

                try
                {
                    var email = _email.SendEmail(new EmailDTO
                    {
                        To = subscriber.Email,
                        Subject = BusinessManager.PasswordChangedSubject,
                        Body = BusinessManager.PasswordChangedBody,
                    });


                }
                catch (Exception ex)
                {
                    //email success, failure, and exception to be handled
                }
                finally
                {
                    result.Message = BusinessManager.PasswordChangedBody;

                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;

            }
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
            var resetDto = new SubscriberResetPasswordDTORequest
            {
                Email = input.Email,
                NewPassword = input.NewPassword,
                ConfirmPassword = input.ConfirmPassword
            };

            var resetResult = await ResetPasswordAsync(resetDto);
            return resetResult;
        }





        #endregion
    }

}
