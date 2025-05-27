using Microsoft.AspNetCore.Mvc;

namespace eSim.Selfcare.Components.DashbodBundlByMonth
{
    public class BundelByMonthViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
