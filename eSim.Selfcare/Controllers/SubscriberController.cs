using eSim.Common.Extensions;
using eSim.Common.StaticClasses;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Selfcare.Subscriber;
using eSim.Infrastructure.Interfaces.Middleware;
using eSim.Infrastructure.Interfaces.Selfcare.Refrence;
using eSim.Infrastructure.Interfaces.Selfcare.Subscriber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eSim.Selfcare.Controllers
{
    [Authorize]
    public class SubscriberController : Controller
    { private readonly ISubscriber _Subscriber;
        private readonly ICountyService _countryService;
        public SubscriberController(ISubscriber Subscriber, ICountyService countryService)
        {
            _Subscriber = Subscriber;
            _countryService = countryService;
        }
        public IActionResult Subscriber()
        {
            return View();
        }
        public async Task<IActionResult> Detail()
        {
            var result = await _Subscriber.SubscriberDetailAsync();

            if (!result.Success || result.Data == null)
                return NotFound();

            var countries = await _countryService.Countries();
            if (countries != null)
            {
                result.Data.CountryList = countries.Select(x => new CountriesDTORequest
                {
                    CountryName = x.CountryName,
                    Iso2 = x.Iso2,
                    Iso3 = x.Iso3
                }).ToList();
            }
          //  HttpContext.Session.SetString("ProfileImage", string.IsNullOrWhiteSpace(result.Data.ProfileImage)
          //? Url.Content("~/assets/images/users/avatar-1.jpg")
          //: "https://localhost:7264" + result.Data.ProfileImage);

          //  HttpContext.Session.SetString("FullName", result.Data.FirstName + " " + result.Data.LastName);



            return View(result.Data);
        }

        //[HttpGet]
        //public async Task<IActionResult> UpdateSubscriber()
        //{

        //    var result = await _Subscriber.SubscriberDetailAsync();
        //    if (!result.Success || result.Data == null)
        //        return NotFound();

        //    var countries = await _countryService.Countries();

        //    if (countries != null)
        //    {
        //        result.Data.CountryList = countries.Select(x => x.CountryName).ToList();
        //    }

        //    return View(result.Data);


        //}
        [HttpPost]
        public async Task<IActionResult> UpdateSubscriber( UpdateSubscriberDTORequest request)
        {
            var result = await _Subscriber.UpdateSubscriberAsync(request);
            return RedirectToAction(nameof(Detail));

        }
     


        [HttpPost]
        public async Task<IActionResult> ChangePassword( ChangePasswordDTORequest request)
        {
            var result=await _Subscriber.ChangePasswordAsync(request);
            return RedirectToAction(nameof(Detail));
        }
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please upload a valid image.";
                return RedirectToAction(nameof(Detail)); 
            }

            var result = await _Subscriber.UploadProfileImage(file); 

            if (result.Success)
                TempData["Success"] = "Image uploaded successfully!";
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Detail));
        }



    }
}
