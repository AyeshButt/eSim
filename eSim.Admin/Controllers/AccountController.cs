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
using eSim.Infrastructure.DTOs;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Email;
using eSim.EF.Entities;
using eSim.Infrastructure.Interfaces.Admin.Account;
using eSim.Infrastructure.Interfaces.Admin.Email;
using static System.Net.WebRequestMethods;
using System.ComponentModel.DataAnnotations;
using eSim.Common.StaticClasses;

namespace eSim.Admin.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        IConfiguration _config;
        private readonly IAccountService _account;
        private readonly IEmailService _email;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration config, IAccountService account, IEmailService email)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _account = account;
            _email = email;
        }

        [HttpGet]
        public IActionResult Index(string? ReturnUrl)
        {
            var model = new LoginDTO();
            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                model.ReturnUrl = ReturnUrl;
            return View(model: model);
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
                    return View();
                }

            }
            TempData["LoginFailed"] = "Enter correct email or password.";
            return View(model: input);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(model: new RegisterDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO input)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = input.Email,
                    UserName = input.Email,
                };
                var registerResult = await _userManager.CreateAsync(user: user, password: input.Password); // pass the plain password, Identity will hash it self

                if (registerResult.Succeeded)
                {
                    // on success creation will login the user automatically
                    var signInResult = await _signInManager.PasswordSignInAsync(userName: input.Email, password: input.Password, isPersistent: true, lockoutOnFailure: false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                    }
                }
                else
                {
                    foreach (var error in registerResult.Errors)
                    {
                        ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
                    }

                }

            }

            return View(model: input);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(actionName: "Index");
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

                return RedirectToAction(nameof(ForgotPassword));
            }

            var user = await _account.VerifyEmail(input.Email);

            if (user is null)
            {
                TempData["UserNotFound"] = BusinessManager.UserNotFound;

                return RedirectToAction(nameof(ForgotPassword));
            }

            #region 6 Digit OTP

            string randomNumber = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            #endregion

            var OTPDetails = new OTPVerificationDTO()
            {
                UserId = user.Id,
                IsValid = true,
                Type = OTPType.ForgotPassword.ToString(),
                SentTime = DateTime.Now,
                OTP = randomNumber,
                Email = user.Email,
            };

            TempData["OTPDetails"] = JsonConvert.SerializeObject(OTPDetails);

            #region SendEmail

            EmailDTO email = new EmailDTO();

            email.To = "ayeshbutt012@gmail.com";
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

            if (OTP_Validity is not null && OTP_Validity.IsValid == true)
            {
                return View();
            }

            TempData["ResetLinkExpired"] = BusinessManager.LinkExpired;

            return RedirectToAction("ForgotPassword", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> OTP(OTPVerificationDTO otpDetails)
        {
            if (!string.IsNullOrEmpty(otpDetails.OTP))
            {
                var verifyOTP = await _account.VerifyOTP(otpDetails);

                if (verifyOTP is not null)
                {

                    return RedirectToAction("ResetPassword", "Account", new { id = verifyOTP.UserId });
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
                return RedirectToAction("ForgotPassword", "Account");

            }

            #endregion

            return View(reset);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO input)
        {
            if (!ModelState.IsValid || input.NewPassword != input.ConfirmPassword || await _userManager.FindByIdAsync(input.UserId) is not ApplicationUser user)
            {
                return RedirectToAction("ForgotPassword", "Account");
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
        [HttpGet]
        public IActionResult Email()
        {
            return View(new EmailEntity());
        }
        [HttpPost]
        public IActionResult Email(EmailEntity objemailEntity)
        {

            //There are two approaches to send an email through SMTP and MailKit/MimeKit

            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var username = myAppConfig.GetValue<string>("EmailConfig:Username");
            var password = myAppConfig.GetValue<string>("EmailConfig:password");
            var host = myAppConfig.GetValue<string>("EmailConfig:Host");
            //var port = myAppConfig.GetValue<int>("EmailConfig:port");
            var fromEmail = myAppConfig.GetValue<string>("EmailConfig:FromEmail");


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Contact", fromEmail));
            message.To.Add(new MailboxAddress("Recipient", objemailEntity.ToEmailAddress));
            message.Subject = objemailEntity.Subject;
            //message.Body = new TextPart("plain")
            //{
            //    Text = objemailEntity.EmailBody
            //};

            message.Body = new TextPart("html")
            {
                Text = @"
        <html>
        <head>
            <style>
                body { font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333; padding: 20px; }
                h1 { color: #5b9bd5; }
                p { line-height: 1.6; }
                .footer { font-size: 12px; color: #888; margin-top: 20px; }
            </style>
        </head>
        <body>
            <h1>" + objemailEntity.Subject + @"</h1>
            <p>" + objemailEntity.EmailBody + @"</p>
            <div class='footer'>
                <p>Thank you for reaching out!</p>
            </div>
        </body>
        </html>"
            };

            //var message = new MailMessage();

            //message.From = new MailAddress(fromEmail);
            //message.To.Add(objemailEntity.ToEmailAddress.ToString());
            //message.Subject = objemailEntity.Subject;
            //message.IsBodyHtml = true;
            //message.Body = objemailEntity.EmailBody;

            using (var client = new SmtpClient())
            {

                try
                {
                    client.Connect(host, 465, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(fromEmail, password);
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                    client.Disconnect(true);

                }

                //SmtpClient smtp = new SmtpClient(host);
                //try
                //{

                //    smtp.UseDefaultCredentials = false;
                //    smtp.Credentials = new System.Net.NetworkCredential(username, password);
                //    smtp.Host = host;
                //    smtp.EnableSsl = true;
                //    smtp.Port = port;
                //    smtp.Send(message);

                //}



                catch(Exception ex)
                {
                    TempData["Exception"] = $"Exception occured: {ex.Message}";
                    return View();
                }
                finally
                {
                    client.Dispose();
                }
            }

            TempData["EmailSent"] = $"Email sent successfully";

            return RedirectToAction("Email");
        }
    }
}


public class EmailEntity
{
    [Required(ErrorMessage ="Email is required")]
    [DataType(DataType.EmailAddress)]
    public string FromEmailAddress { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string ToEmailAddress { get; set; }

    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Message body is required")]
    public string EmailBody { get; set; }


}
