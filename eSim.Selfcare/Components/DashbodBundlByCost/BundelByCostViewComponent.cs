using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Components.DashbodBundlByCost
{
    public class BundelByCostViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            return View();
        }
       
    }
}
