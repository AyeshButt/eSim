using eSim.Implementations.Services.Middleware.Subscriber;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriberController(ISubscriberService subscriber,IEmailService email,IForgotPassword password) : ControllerBase
    {
        private readonly ISubscriberService _subscriber = subscriber;
        private readonly IEmailService _emailService = email;
        private readonly IForgotPassword _password = password;
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required.");
            }

            var exists = await _subscriber.EmailExists(email);

            if (exists)
            {
                return Ok("Email already exists.");
            }

            return Ok("Email is available.");
        }
        #region Subscriber

        [HttpPost]

        public IActionResult POST(SubscriberRequestDTO input)
        {
            if (ModelState.IsValid)
            {
                var result = _subscriber.CreateSubscriber(input).GetAwaiter().GetResult();
                if (result.Success)
                {
                    var email = new EmailDTO
                    {
                        To = input.Email, 
                        Body = $"Hi {input.FirstName},\n\nYou are successfully signed up on our platform.\n\nThanks,\neSim Team"
                    };

                    var emailResult = _emailService.SendEmail(email).GetAwaiter().GetResult();
                    if (!emailResult.Success)
                    {
                        Console.WriteLine("Email error: " + emailResult.Data);
                     
                    }
                    return Ok(input);
                }
                else
                {
                    return Problem(result.Data);
                }
            }
            else
            {

                return BadRequest();
            }

        }
        #endregion
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _password.ForgotPasswordAsync(model);

            if (!result.Success)
                return BadRequest(result.Data);

            return Ok(result.Data);
        }
        [AllowAnonymous]
        [HttpGet("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] string otp)
        {
            if (string.IsNullOrEmpty(otp))
                return BadRequest("OTP is required.");

            var result = await _password.VerifyOtpAsync(otp);

            if (!result.Success)
                return BadRequest(result.Data);

            return Ok(result.Data);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _password.ResetPasswordAsync(model);

            if (!result.Success)
                return BadRequest(result.Data);

            return Ok(result.Data);
        }
    }
}
