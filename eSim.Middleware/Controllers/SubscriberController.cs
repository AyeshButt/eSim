using eSim.Common.Extensions;
using eSim.Common.StaticClasses;
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
    public class SubscriberController(ISubscriberService subscriber, IForgotPassword password) : ControllerBase
    {
        private readonly ISubscriberService _subscriber = subscriber;
        private readonly IForgotPassword _password = password;

        [AllowAnonymous]
        [HttpGet("email-exists")]
        public async Task<IActionResult> CheckSubscriberEmailExists([FromQuery] string email)
        {
            var result = await _subscriber.SubscriberEmailExists(email);
            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        

        }

        #region Subscriber  
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubscriberDTORequest input)
        {
            var result = await _subscriber.CreateSubscriber(input);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        #endregion
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTORequest input)
        {
            var result = await _password.ForgotPasswordAsync(input);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);

        }

        [AllowAnonymous]
        [HttpGet("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromQuery] string otp)
        {
            var result = await _password.VerifyOTPAsync(otp);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] SubscriberResetPasswordDTORequest input)
        {
            var result = await _password.ResetPasswordAsync(input);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        [AllowAnonymous]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTORequest input)
        {
           

            var result = await _password.ChangePasswordAsync(input);
            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetSubscriberDetail()
        {   
            var loggedUser=User.SubscriberId();

            if (loggedUser is null)
                return Unauthorized();

            var result = await _subscriber.GetSubscriberDetailAsync(Guid.Parse(loggedUser));
            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }

        
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateSubscriber([FromBody] UpdateSubscriberDTORequest input)

        {
            var loggeduser=User.SubscriberId();
            if(loggeduser is null)
                return Unauthorized();
            var result = await _subscriber.UpdateSubscriberAsync(Guid.Parse(loggeduser), input);
            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }



        [AllowAnonymous]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file, [FromForm] Guid subscriberId)
        {
            

            var dto = new ProfileImageDTORequest
            {
                SubscriberId = subscriberId
            };

            var result = await _subscriber.UploadProfileImageAsync(file, dto);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }





    }
}
