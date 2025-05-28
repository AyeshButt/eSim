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
            var result = await _subscriber.EmailExists(email);

            // HTTP 400 if failed
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
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
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] string otp)
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
        public async Task<IActionResult> ResetPassword([FromBody] SubscriberResetPasswordDTO model)
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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _subscriber.GetSubscriber(id);

            if (result == null)
                return StatusCode(500, "An unexpected error occurred.");

            if (result.Success)
                return Ok(result);

            return BadRequest(result.Message);
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
