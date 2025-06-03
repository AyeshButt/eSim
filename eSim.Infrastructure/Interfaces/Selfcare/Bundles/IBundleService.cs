using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Infrastructure.Interfaces.Selfcare.Bundles
{
    public interface IBundleService
    {
        public Task<Result<GetBundleCatalogueResponse>> GetBundles();
        public Task<Result<GetBundleCatalogueDetail>> BundleDetail(BundleNameDTO input);

    }
}
