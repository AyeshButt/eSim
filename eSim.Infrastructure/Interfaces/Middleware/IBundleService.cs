using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface IBundleService
    {
        Task<Result<GetBundleCatalogueResponse>> GetBundlesAsync();
    }
}
