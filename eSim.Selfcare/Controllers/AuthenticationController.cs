using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Selfcare.Authentication;
using eSim.Infrastructure.Interfaces.Selfcare.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ISignIn _auth;

        public AuthenticationController(ISignIn auth)
        {
            _auth = auth;
        }


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
            
            if (ModelState.IsValid) 
            {
                var isAuthenticated = await _auth.AuthenticateAsync(model);

                if (isAuthenticated)
                {
                    return RedirectToAction( actionName : "Index", controllerName:"Dashboard");
                }
                
            }
            TempData["LoginFailed"] = BusinessManager.LoginFailed;

            return View(model: model);
        }

        
        public IActionResult SignInCover()
        {
            return View();
        }

        [ActionName("SignUpBasic")]
        public IActionResult SignUpBasic()
        {
            return View();
        }

        [ActionName("SignUpCover")]
        public IActionResult SignUpCover()
        {
            return View();
        }

        [ActionName("PasswordChangeBasic")]
        public IActionResult PasswordChangeBasic()
        {
            return View();
        }

        [ActionName("PasswordChangeCover")]
        public IActionResult PasswordChangeCover()
        {
            return View();
        }

        [ActionName("PasswordResetBasic")]
        public IActionResult PasswordResetBasic()
        {
            return View();
        }

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

        [ActionName("LogoutBasic")]
        public IActionResult LogoutBasic()
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
