using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Components.DashbodBundlByCountry
{
    public class BundelByCountryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
