using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Components.DashbodBundlByRegion
{
    public class BundelByRegionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}



