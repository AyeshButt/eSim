using eSim.Common.Enums;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using MimeKit;
using Newtonsoft.Json;
using eSim.Common;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Email;
using eSim.EF.Entities;
using eSim.Infrastructure.Interfaces.Admin.Account;
using eSim.Infrastructure.Interfaces.Admin.Email;
using static System.Net.WebRequestMethods;
using System.ComponentModel.DataAnnotations;
using eSim.Common.StaticClasses;
using Microsoft.Extensions.Options;
using eSim.Infrastructure.DTOs.Configuration;
using System.Security.Claims;

namespace eSim.Admin.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IAccountService _account;
        private readonly IEmailService _email;
        private readonly IOptions<EmailConfig> _options;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration config, IAccountService account, IEmailService email, IOptions<EmailConfig> options)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _account = account;
            _email = email;
            _options = options;
        }

        [HttpGet]
        public IActionResult Configs()
        {
            ConfigDTO model = new ConfigDTO();

            var key = _config.GetValue("KeyId", "DEFAULT");

            model.ConnectionString = _config.GetConnectionString("AppDbConnection") ?? "unable to located";
            model.KeyId = key;

            return View(model: model);
        }
        [HttpGet]
        public IActionResult Index(string? ReturnUrl)
        {
            var model = new LoginDTO();

            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                model.ReturnUrl = ReturnUrl;

            return View(model: model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO input)
        {
            if (ModelState.IsValid)
            {

                var signInResult = await _signInManager.PasswordSignInAsync(userName: input.Email, password: input.Password, isPersistent: true, lockoutOnFailure: false);

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(input.ReturnUrl) && Url.IsLocalUrl(input.ReturnUrl))
                    {
                        return RedirectToAction(input.ReturnUrl);
                    }
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }

                if (signInResult.IsLockedOut)
                {
                    TempData["LockedOut"] = BusinessManager.LockedOut;
                    return RedirectToAction("Index");
                }

            }

            TempData["LoginFailed"] = BusinessManager.LoginFailed;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordDTO());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO input)
        {
            if (string.IsNullOrEmpty(input.Email))
            {
                TempData["EmailValidationError"] = BusinessManager.EmailValidationError;

                return RedirectToAction("ForgotPassword");
            }

            var user = await _account.VerifyEmail(input.Email);

            if (user.Data is null)
            {
                TempData["UserNotFound"] = BusinessManager.UserNotFound;

                return RedirectToAction("ForgotPassword");
            }

            #region 6 Digit OTP

            string randomNumber = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            #endregion

            var OTPDetails = new OTPVerificationDTO()
            {
                UserId = user.Data.Id,
                IsValid = true,
                Type = OTPType.ForgotPassword.ToString(),
                SentTime = DateTime.Now,
                OTP = randomNumber,
                Email = user.Data.Email,
            };

            TempData["OTPDetails"] = JsonConvert.SerializeObject(OTPDetails);

            #region SendEmail

            EmailDTO email = new EmailDTO();

            email.To = OTPDetails.Email;
            email.Subject = BusinessManager.EmailSubject;
            email.Body = BusinessManager.EmailBody + OTPDetails.OTP;

            var IsEmailSent = await _email.SendEmail(email);
            #endregion

            if (IsEmailSent.Success)
            {
                var result = await _account.AddOTPDetails(OTPDetails);

                if (result.Success)
                {
                    return RedirectToAction(nameof(OTP), new { id = OTPDetails.UserId });
                }

                TempData["OTPDetailsNotAdded"] = BusinessManager.OTPDetailsNotAdded;

                return RedirectToAction(nameof(ForgotPassword));

            }

            TempData["EmailNotReceived"] = BusinessManager.EmailNotReceived;

            return RedirectToAction(nameof(ForgotPassword));
        }

        [HttpGet]
        public async Task<IActionResult> OTP(string id)
        {
            var otp = TempData["OTPDetails"]?.ToString();

            if (otp is not null)
            {
                var OTPDetails = JsonConvert.DeserializeObject<OTPVerificationDTO>(otp);

                if (OTPDetails is not null)
                {
                    return View();
                }
            }

            var OTP_Validity = await _account.GetValidOTPDetails(id);

            if (OTP_Validity.Success && OTP_Validity.Data is not null && OTP_Validity.Data.IsValid == true)
            {
                return View();
            }


                return RedirectToAction("ForgotPassword");
        }

        [HttpPost]
        public async Task<IActionResult> OTP(OTPVerificationDTO otpDetails)
        {
            if (!string.IsNullOrEmpty(otpDetails.OTP))
            {
                var verifyOTP = await _account.VerifyOTP(otpDetails);

                if (verifyOTP.Success && verifyOTP.Data is not null)
                {

                    return RedirectToAction("ResetPassword", "Account", new { id = verifyOTP.Data.UserId });
                }

                TempData["OTPFailed"] = BusinessManager.OTPFailed;
            }

            return RedirectToAction("OTP", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {

            TempData["ResetLinkExpired"] = BusinessManager.LinkExpired;

            ResetPasswordDTO reset = new ResetPasswordDTO()
            {
                UserId = id
            };


            #region Remove Existing OTP

            var isDeleted = await _account.RemoveOTPDetails(id);

            if (!isDeleted.Success)
            {
                return RedirectToAction("ForgotPassword");

            }

            #endregion

            return View(reset);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO input)
        {
            if (!ModelState.IsValid || input.NewPassword != input.ConfirmPassword || await _userManager.FindByIdAsync(input.UserId) is not ApplicationUser user)
            {
                return RedirectToAction("ForgotPassword");
            }

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);

            if (removePasswordResult.Succeeded)
            {
                var addPasswordResult = await _userManager.AddPasswordAsync(user, input.NewPassword);

                if (addPasswordResult.Succeeded)
                {
                    TempData["PasswordSuccessfullyReset"] = BusinessManager.PasswordSuccessfullyReset;
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                foreach (var error in removePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View();
        }
       
        #region Testing Email
        [HttpGet]
        public IActionResult Email()
        {
            return View(new EmailDTO());
        }
        [HttpPost]
        public async Task<IActionResult> Email(EmailDTO input)
        {
            var email = await _email.SendEmail(input);

            if (email.Success)
            {
                TempData["EmailSent"] = BusinessManager.EmailSent;

                return RedirectToAction("Email");
            }

            TempData["Exception"] = email.Data;

            return View();
        }
        #endregion
    }

}



