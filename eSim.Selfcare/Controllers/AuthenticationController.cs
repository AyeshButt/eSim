using System.Security.Claims;
using System.Threading.Tasks;
using Azure;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Selfcare.Authentication;
using eSim.Infrastructure.DTOs.Selfcare.Subscriber;
using eSim.Infrastructure.Interfaces.Selfcare.Authentication;
using eSim.Infrastructure.Interfaces.Selfcare.Refrence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly Infrastructure.Interfaces.Selfcare.Authentication.IAuthenticationService _auth;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ICountyService _countryService;

        public AuthenticationController(Infrastructure.Interfaces.Selfcare.Authentication.IAuthenticationService auth, IHttpContextAccessor httpContext, ICountyService countryService)
        {
            _auth = auth;
            _httpContext = httpContext;
            _countryService = countryService;
        }

        public new HttpContext HttpContext => _httpContext.HttpContext!;


        #region Login for User

        [HttpGet]
        public IActionResult SignIn()
        {
            var model = new SignIn();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }
            
            var token = await _auth.AuthenticateAsync(model);
    

            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("Token", token);

                //creating claim 

                var claims = new List<Claim>
                {
                    new Claim("Token", token)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principle  = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);


                return RedirectToAction("Index", "Dashboard");
                
            }

            TempData["ToastMessage"] = " Error! Login Failed";
            TempData["ToastType"] = false;

            return View(model);
        }


        #endregion

        #region SignUp

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> SignUp()
        {
            var country =await  _countryService.Countries();

            if (country != null)
            {
                ViewBag.Country = country;
            }

            return View();
        }


        #region Check Email

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEmail(string email) 
        {
            var request = await _auth.Email(email);
            var resp = request.Data;   
            
            if (!string.IsNullOrEmpty(request.Message) && request.Message.Contains("Email already exists."))
            {
                return Json(false);
            }

            return Json(true);
            
        }
        #endregion

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> SignUp(SubscriberViewModel input)
        {     
            if (ModelState.IsValid)
            {
                var response = await _auth.Create(input);

                if (response.Success) 
                {
                    TempData["ToastMessage"] =response.Message;
                    TempData["ToastType"] = response.Success;
                    return RedirectToAction("SignIn");
                }

                TempData["ToastMessage"] = response.Message;
                TempData["ToastType"] = response.Success;

            }

            return View();
        }

        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            
            return View();
        }

        #endregion


        #region Forgot Password

        [HttpGet]
        public IActionResult PasswordReset()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(ForgotPasswordDTORequest input)
        {
            if (ModelState.IsValid)
            {
                var response = await _auth.ForgotPassword(input);

                if (response.Success) 
                {
                    TempData["Email"] = input.Email;
                    TempData["ToastMessage"] = response.Message;
                    TempData["ToastType"] = response.Success;
                    return RedirectToAction("TwoStepVerification");
                }
                else
                {
                    TempData["ToastMessage"] = response.Message;
                    TempData["ToastType"] = response.Success;
                }
               
            }
            return View();
        }

        #endregion


        #region TwoStep Verification

        [HttpGet]
        public IActionResult TwoStepVerification()
        {
            if (TempData["Email"] != null)
            {
                string email = TempData["Email"].ToString();
                TempData.Keep("Email");
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TwoStepVerification(otpDTO input)
        {
            if (ModelState.IsValid)
            {
              
                var OTP = input.FullOtp;
                var response = await _auth.OTPVarification(OTP);

                if (response.Success)
                {
                    var email = TempData["Email"]?.ToString();
                    TempData["Email"] = email;

                    TempData["ToastMessage"] = response.Message;
                    TempData["ToastType"] = response.Success;
                    return RedirectToAction("PasswordChange");
                }

                TempData["ToastMessage"] = response.Message;
                TempData["ToastType"] = response.Success;
            }
            return View();
        }

        #endregion

        #region Change Password 

        [HttpGet]
        public IActionResult PasswordChange()
        {
            if (TempData["Email"] != null)
            {

                string email = TempData["Email"].ToString();

                //Console.WriteLine(email);
                ViewBag.Email = email;
                return View();
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PasswordChange(SubscriberResetPasswordDTORequest input)
        {

            if (ModelState.IsValid) 
            {
                var response = await _auth.NewPassword(input);

                if (response.Success) 
                {
                    TempData["ToastMessage"] = response.Message;
                    TempData["ToastType"] = response.Success;
                    return RedirectToAction("SignIn");
                }
            }
            return View();
        }

        #endregion


        [ActionName("PasswordResetCover")]
        public IActionResult PasswordResetCover()
        {
            return View();
        }

        [ActionName("LockScreenBasic")]
        public IActionResult LockScreenBasic()
        {
            return View();
        }

        [ActionName("LockScreenCover")]
        public IActionResult LockScreenCover()
        {
            return View();
        }



        [ActionName("LogoutCover")]
        public IActionResult LogoutCover()
        {
            return View();
        }

        [ActionName("SuccessMessageBasic")]
        public IActionResult SuccessMessageBasic()
        {
            return View();
        }

        [ActionName("SuccessMessageCover")]
        public IActionResult SuccessMessageCover()
        {
            return View();
        }

        [ActionName("TwoStepVerificationBasic")]
        public IActionResult TwoStepVerificationBasic()
        {
            return View();
        }

        [ActionName("TwoStepVerificationCover")]
        public IActionResult TwoStepVerificationCover()
        {
            return View();
        }

        [ActionName("Errors404Basic")]
        public IActionResult Errors404Basic()
        {
            return View();
        }

        [ActionName("Errors404Cover")]
        public IActionResult Errors404Cover()
        {
            return View();
        }

        [ActionName("Errors404Alt")]
        public IActionResult Errors404Alt()
        {
            return View();
        }

        [ActionName("Errors500")]
        public IActionResult Errors500()
        {
            return View();
        }

        [ActionName("Offline")]
        public IActionResult Offline()
        {
            return View();
        }
    }
}
