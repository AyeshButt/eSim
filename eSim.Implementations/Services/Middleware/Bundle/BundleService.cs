using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware;

namespace eSim.Implementations.Services.Middleware.Bundle
{
    public class BundleService : IBundleService
    {
        private readonly IConsumeApi   _consumeApi;

        public BundleService(IConsumeApi consumeApi)
        {
            _consumeApi = consumeApi;
        }

        public async Task<Result<GetBundleCatalogueResponse>> GetBundlesAsync()
        {
            var result = new Result<GetBundleCatalogueResponse>();

            string url = $"{BusinessManager.BaseURL}/catalogue?page=2&perPage=50&direction=desc&orderBy=speed&region=Asia";

            try
            {
                var response = await _consumeApi.GetApi<GetBundleCatalogueResponse>(url);

                if (response == null)
                {
                    result.Success = false;
                    result.Data = null;
                    return result;
                }

                result.Success = true;
                result.Data = response;
               
            }
            catch (Exception)
            {
                result.Success = false;
                result.Data = null;
            }

            return result;
        }

    }
}

