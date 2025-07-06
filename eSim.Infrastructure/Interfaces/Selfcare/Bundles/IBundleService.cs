using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.DTOs.Selfcare.Bundles;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;


namespace eSim.Infrastructure.Interfaces.Selfcare.Bundles
{
    public interface IBundleService
    {
        public Task<Result<BundleViewModel>> GetBundles();
        public Task<Result<GetBundleCatalogueDetailsResponse>> BundleDetail(string input);
        public Task<Result<CreateOrderResponse>> CreateOrderAsync(OrderModalViewModel input);

    }
}
