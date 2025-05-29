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

        public async Task<Result<List<Bundle>>> GetBundles()
        {
            var Url = BusinessManager.MdwBaseURL + BusinessManager.BundelRegion;

            var response = await _consumeApi.Get<List<Bundle>>(Url);

            return response;
        }
    }
}
