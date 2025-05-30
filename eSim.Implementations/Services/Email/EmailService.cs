//using eSim.Common.StaticClasses;
//using eSim.EF.Context;
//using eSim.EF.Entities;
//using eSim.Infrastructure.DTOs.Client;
//using eSim.Infrastructure.DTOs.Email;
//using eSim.Infrastructure.DTOs.Global;
//using eSim.Infrastructure.Interfaces.Admin.Email;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using MimeKit;
//using System.Buffers.Text;
//using System.Text;

//namespace eSim.Implementations.Services.Email
//{
//    public class EmailService : IEmailService
//    {
//        private readonly ApplicationDbContext _db;
//        private readonly IOptions<EmailConfig> _options;
//        private readonly IConfiguration _config;
//        private readonly UserManager<ApplicationUser> _userManager;


//        public EmailService(ApplicationDbContext db, IOptions<EmailConfig> options, IConfiguration config, UserManager<ApplicationUser> userManager)
//        {
//            _db = db;
//            _options = options;
//            _config = config;
//            _userManager = userManager;
//        }

//        public Result<string> SendEmail(EmailDTO input)
//        {
//            var result = new Result<string>();

//            var fromEmail = _options.Value.FromEmail;
//            var username = _options.Value.Username;
//            var password = _options.Value.Password;
//            var host = _options.Value.Host;

//            try
//            {
//                var message = new MimeMessage();

//                message.From.Add(new MailboxAddress("Contact", fromEmail));
//                message.To.Add(new MailboxAddress("Recipient", input.To));
//                message.Subject = input.Subject;
//                message.Body = new TextPart("html")
//                {
//                    Text = input.Body
//                };

//                using (var client = new SmtpClient())
//                {
//                    try
//                    {

//                        client.Connect(host, 465, SecureSocketOptions.SslOnConnect);
//                        client.Authenticate(fromEmail, password);
//                        client.Send(message);

//                        Console.WriteLine("Email sent successfully.");

//                        client.Disconnect(true);

//                        result.Success = true;

//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("Error sending email: " + ex.Message);

//                        result.Data = ex.Message;
//                        result.Success = false;

//                        return result;
//                    }
//                    finally
//                    {
//                        client.Dispose();
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                result.Data = ex.Message;
//                result.Success = false;
//            }

//            return result;
//        }

//        public bool SendConfirmationEmail(string primaryEmail, ClientUserDTO input)
//        {
//            var baseurl = GetBaseUrl();

//            if (baseurl is null)
//                return false;

//            EmailDTO email = new EmailDTO()
//            {
//                To = primaryEmail,
//                Subject = BusinessManager.Verification_EmailSubject,
//                Body = BusinessManager.Verification_EmailBody(input.UserId, baseurl, input.Token ?? string.Empty),
//            };

//            var sendEmail = SendEmail(email);

//            return sendEmail.Success ? true : false;
//        }

//        public bool SendPasswordEmail(string primaryEmail, ClientUserDTO input)
//        {
//            EmailDTO email = new EmailDTO()
//            {
//                To = primaryEmail,
//                Subject = BusinessManager.Password_EmailSubject,
//                Body = input.Password,
//            };

//            var sendEmail = SendEmail(email);

//            return sendEmail.Success ? true : false;
//        }
//        public async Task<string?> EmailConfirmationToken(string userId)
//        {
//            string token = string.Empty;
//            string encodedToken = string.Empty;

//            var findUser = await _userManager.FindByIdAsync(userId);

//            if (findUser is null)
//            {
//                return null;
//            }

//            token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);

//            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
//        }
//        private string? GetBaseUrl()
//        {
//            return _config.GetValue<string>("VerificationEmail:url") ?? null;
//        }

//    }
//}
