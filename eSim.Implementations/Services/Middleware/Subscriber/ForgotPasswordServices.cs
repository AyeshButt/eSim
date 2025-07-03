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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Metrics;
using eSim.Infrastructure.DTOs.Subscribers;

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
                    result.StatusCode = StatusCodes.Status400BadRequest;
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
                    result.StatusCode = StatusCodes.Status200OK;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;

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
                result.StatusCode = StatusCodes.Status400BadRequest;
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
                    result.StatusCode = StatusCodes.Status400BadRequest;

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
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }

            return result;
        }

        #endregion

        #region ResetPassword

        public async Task<Result<string>> ResetPasswordAsync(Guid logged, SubscriberResetPasswordDTO input)
        {
            var result = new Result<string>();

            try
            {
                var subscriber = await _db.Subscribers.FirstOrDefaultAsync(x => x.Id == logged);

                if (subscriber == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.SubscriberNotFound;
                    result.StatusCode = StatusCodes.Status400BadRequest;

                    return result;
                }

                bool isSamePassword = PasswordHasher.VerifyPassword(input.NewPassword, subscriber.Hash);
                if (isSamePassword)
                {
                    result.Success = false;
                    result.Message = "New password cannot be the same as the current password.";
                    result.StatusCode = StatusCodes.Status400BadRequest;
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
                    result.StatusCode = StatusCodes.Status200OK;

                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;

            }
            return result;
        }



        #endregion

        #region changePassowrd


        public async Task<Result<string>> ChangePasswordAsync(Guid logged, ChangePasswordDTORequest input)
        {
            var result = new Result<string>();
            try
            {
                var user = await _db.Subscribers.FirstOrDefaultAsync(u => u.Id == logged);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.userNotFound;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }

          
                bool isOldPasswordCorrect = PasswordHasher.VerifyPassword(input.OldPassword, user.Hash);

                if (!isOldPasswordCorrect || input.NewPassword != input.ConfirmPassword)
                {
                    result.Success = false;
                    result.Message = !isOldPasswordCorrect
                        ? BusinessManager.IncorrectOldPassword
                        : BusinessManager.PasswordNotMatched;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }
                bool isNewSameAsOld = PasswordHasher.VerifyPassword(input.NewPassword, user.Hash);
                if (isNewSameAsOld)
                {
                    result.Success = false;
                    result.Message = "New password cannot be the same as the old password. Please choose a different password.";
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }
                var resetDto = new SubscriberResetPasswordDTO
                {
                    NewPassword = input.NewPassword,
                    ConfirmPassword = input.ConfirmPassword
                };

                result = await ResetPasswordAsync(logged,resetDto);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return result;
        
        #endregion
    }
    }
}


