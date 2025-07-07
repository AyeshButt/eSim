using eSim.Common.Extensions;
using eSim.Common.StaticClasses;
using eSim.Implementations.Services.Middleware.Subscriber;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Database.Extensions;
using Raven.Database.FileSystem.Synchronization.Rdc.Wrapper;

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
        public async Task<IActionResult> ResetPassword([FromBody] SubscriberResetPasswordDTO input)
        {
            var logged = User.SubscriberId();
            if (logged == null)
                return Unauthorized();
            var result = await _password.ResetPasswordAsync(Guid.Parse(logged) ,input);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }


        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTORequest input)
        {
            var logged = User.SubscriberId();
            if(logged == null)
                return Unauthorized();

            var result = await _password.ChangePasswordAsync(Guid.Parse(logged),input);
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




        [HttpPost("Upload")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {


            var loggeduser = User.SubscriberId();
            if(loggeduser is null) return Unauthorized();
            var dto = new ProfileImageDTORequest();
            var result = await _subscriber.UploadProfileImageAsync(Guid.Parse(loggeduser),file,dto);

            return StatusCode(HttpStatusCodeMapper.FetchStatusCode(result.StatusCode), result);
        }





    }
}
