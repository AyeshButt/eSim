using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Admin.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace eSim.Implementations.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _db;
        private readonly IOptions<EmailConfig> _options;

        public EmailService(ApplicationDbContext db, IOptions<EmailConfig> options)
        {
            _db = db;
            _options = options;
        }

        public Result<string> SendEmail(EmailDTO input)
        {
            var result = new Result<string>();

            var fromEmail = _options.Value.FromEmail;
            var username = _options.Value.Username;
            var password = _options.Value.Password;
            var host = _options.Value.Host;

            try
            {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Contact", fromEmail));
            message.To.Add(new MailboxAddress("Recipient", input.To));
            message.Subject = input.Subject;
            message.Body = new TextPart("html")
            {
                Text = input.Body
            };

                using (var client = new SmtpClient())
                {
                    try
                    {

                        client.Connect(host, 465, SecureSocketOptions.SslOnConnect);
                        client.Authenticate(fromEmail, password);
                        client.Send(message);

                        Console.WriteLine("Email sent successfully.");
                        
                        client.Disconnect(true);

                        result.Success = true;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error sending email: " + ex.Message);

                        result.Data = ex.Message;

                        return result;
                    }
                    finally
                    {
                        client.Dispose();
                    }
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
