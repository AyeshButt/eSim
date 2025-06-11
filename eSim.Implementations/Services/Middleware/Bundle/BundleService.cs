using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Mvc;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Implementations.Services.Middleware.Bundle
{
    public class BundleService : IBundleService
    {

        private readonly ApplicationDbContext _db;

        private readonly IConsumeApi _consumeApi;

        public BundleService(IConsumeApi consumeApi, ApplicationDbContext db)
        {
            _consumeApi = consumeApi;
            _db = db;
        }
        #region GetBundleDetail
        public async Task<Result<GetBundleCatalogueDetailsResponse>> GetBundleDetailsAsync(string name)
        {
            var result = new Result<GetBundleCatalogueDetailsResponse>();

            string url = $"{BusinessManager.BaseURL}/catalogue/bundle/{name}";

            try
            {
                var response = await _consumeApi.GetApi<GetBundleCatalogueDetailsResponse>(url);

                if (response == null || response.Message is not null)
                {
                    result.Success = false;
                    result.Message = response?.Message ?? BusinessManager.BundleNotFound;
                    return result;
                }

                result.Data = response;
                result.Message = BusinessManager.BundleFetched;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }


        #endregion
        #region GetBundles
        public async Task<Result<GetBundleCatalogueResponse>> GetBundlesAsync(RegionDTORequest request)
        {
            var result = new Result<GetBundleCatalogueResponse>();
            // Build URL conditionally
            var url = $"{BusinessManager.BaseURL}/catalogue?page={request.Page}&perPage={request.PerPage}&direction={request.Direction}&orderBy={request.OrderBy}&region={request.Region}";

            // Only add countries if it's not null or empty
            if (!string.IsNullOrWhiteSpace(request.Countries) && request.Countries.ToLower() != "string")
            {
                url += $"&countries={request.Countries}";
            }

            try
            {
                var response = await _consumeApi.GetApi<GetBundleCatalogueResponse>(url);

                if (response == null || !response.bundles.Any())
                {
                    result.Success = false;
                    result.Message = BusinessManager.ReagionNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.RegionBundelFetched;


            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }



        #endregion

        public Result<List<CountriesDTORequest>> GetCountries()
        {
            var listOfCountry = _db.Countries.Select(a => new CountriesDTORequest { CountryName = a.CountryName, Iso2 = a.Iso2, Iso3 = a.Iso3 }).ToList();


            return new Result<List<CountriesDTORequest>>()
            {

                Data = listOfCountry,

            };
        }

    }
}

