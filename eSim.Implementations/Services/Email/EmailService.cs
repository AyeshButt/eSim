using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Admin.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace eSim.Implementations.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public EmailService(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<Result> SendEmail(EmailDTO emailDto)
        {
            var fromEmail = _configuration.GetValue<string>("EmailConfig:FromEmail");
            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var username = myAppConfig.GetValue<string>("EmailConfig:Username");
            var password = myAppConfig.GetValue<string>("EmailConfig:password");
            var host = myAppConfig.GetValue<string>("EmailConfig:Host");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Contact", fromEmail));
            message.To.Add(new MailboxAddress("Recipient", emailDto.To));
            message.Subject = emailDto.Subject;
            message.Body = new TextPart("plain")
            {
                Text = emailDto.Body
            };

            using (var client = new SmtpClient())
            {
                try
                {

                    client.Connect(host, 465, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(fromEmail, password);
                    await client.SendAsync(message);
                    Console.WriteLine("Email sent successfully.");
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                    return new Result() { Message = ex.Message };
                }
                finally
                {
                    client.Dispose();
                }
            }
            return new Result() { Success = true };
        }

        public async Task<Result> MAIL(EmailDTO email)
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
            message.To.Add(new MailboxAddress("Recipient", email.To));
            message.Subject = email.Subject;
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
            <h1>" + email.Subject + @"</h1>
            <p>" + email.Body + @"</p>
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



                catch
                {
                }
                finally
                {
                    client.Dispose();
                }
                return new Result() { Success = true };

            }
        }
    }
}
