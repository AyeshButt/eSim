using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Implementations.Services.Selfcare
{
    public class BundleService(IMiddlewareConsumeApi consumeApi) : IBundleService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;

       

        public async Task<Result<GetBundleCatalogueResponse>> GetBundles()
        {
            RegionDTO dto = new()
            {
                Region = "Europe"
            };
            var Url = BusinessManager.MdwBaseURL + BusinessManager.BundelRegion;
            
 
            var response = await _consumeApi.Post<GetBundleCatalogueResponse, RegionDTO>(Url, dto);

            return response;
        }


        public async Task<Result<GetBundleCatalogueDetail>> BundleDetail(string dto)
        {
            var Url = BusinessManager.MdwBaseURL + BusinessManager.Bundeldetail;

            //var fullUrl = $"{Url}?name={HttpUtility.UrlEncode(dto)}";
            var FullURl = $"{Url}?name={Uri.EscapeDataString(dto)}";

            var response = await _consumeApi.Get<GetBundleCatalogueDetail>(FullURl);

            return response;
        }
    }
}
