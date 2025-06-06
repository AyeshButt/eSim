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
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        #region Subscriber  
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubscriberDTORequest input)
        {
            var result = await _subscriber.CreateSubscriber(input);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTORequest input)
        {
            var result = await _password.ForgotPasswordAsync(input);

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);

        }

        [AllowAnonymous]
        [HttpGet("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromQuery] string otp)
        {
            var result = await _password.VerifyOTPAsync(otp);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] SubscriberResetPasswordDTORequest input)
        {
            var result = await _password.ResetPasswordAsync(input);

            return result.Success ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        [AllowAnonymous]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<string>
                {
                    Success = false,
                    Message = "Invalid input.",
                    Data = null
                });
            }

            var result = await _password.ChangePasswordAsync(input);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSubscriber(Guid id, [FromBody] UpdateSubscriberDTO input)
        {
            var result = await _subscriber.UpdateSubscriberAsync(id, input);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }



        [AllowAnonymous]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadProfileImage(IFormFile image, [FromForm] Guid subscriberId)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "No image file provided."
                });
            }

            var dto = new ProfileImageDTO
            {
                SubscriberId = subscriberId
            };

            var result = await _subscriber.UploadProfileImageAsync(image, dto);

            if (!result.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }





    }
}
