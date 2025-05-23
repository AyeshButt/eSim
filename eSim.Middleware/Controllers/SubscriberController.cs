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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubscriberRequestDTO input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<string>
                {
                    Success = false,
                    Message = "Invalid input data"
                });
            }

            var result = await _subscriber.CreateSubscriber(input);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        // code ok


        #endregion
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
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

            var result = await _password.ForgotPasswordAsync(model);

            if (!result.Success)
            {
                return BadRequest(result); 
            }

            return Ok(result); 
        }

        [AllowAnonymous]
        [HttpGet("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                return BadRequest(new Result<string>
                {
                    Success = false,
                    Message = "OTP is required.",
                   
                });
            }

            var result = await _password.VerifyOtpAsync(otp);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
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

            var result = await _password.ResetPasswordAsync(model);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("change-password")]
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
                return Ok(result.Data);

            return BadRequest(result.Data);
        }


        [AllowAnonymous]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadProfileImage([FromBody] FormFile file)
        {
            var s = Request.Form.Files;

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "No image file provided."
                    
                });
            }

            var dto = new ProfileImageDTO();

            var result = await _subscriber.UploadProfileImageAsync(file, dto);

            if (!result.Success)
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }





    }
}
