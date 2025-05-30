using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;

namespace eSim.Implementations.Services.Selfcare
{
    public class BundleService(IMiddlewareConsumeApi consumeApi) : IBundleService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;

        public async Task<Result<List<GetBundleCatalogueResponse>>> GetBundles()
        {
            RegionDTO dto = new()
            {
                Region = "Asia"
            };
            var Url = BusinessManager.MdwBaseURL + BusinessManager.BundelRegion;
            
 
            var response = await _consumeApi.Post< List<GetBundleCatalogueResponse>, RegionDTO>(Url, dto);

            return response;
        }
    }
}
