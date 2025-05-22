using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using Microsoft.AspNetCore.Mvc;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Infrastructure.Interfaces.Middleware
{
    public interface IBundleService
    {
        Task<Result<GetBundleCatalogueResponse>> GetBundlesAsync(string region);
        Task<Result<GetBundleCatalogueDetail>> GetBundleDetailAsync(string name);
        Result<List<CountriesDTO>> GetCountries();


    }
}
