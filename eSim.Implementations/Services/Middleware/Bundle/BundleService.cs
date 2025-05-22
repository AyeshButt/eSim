using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
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

        private readonly IConsumeApi   _consumeApi;

        public BundleService(IConsumeApi consumeApi, ApplicationDbContext db)
        {
            _consumeApi = consumeApi;
            _db = db;
        }
        #region GetBundleDetail
        public async Task<Result<GetBundleCatalogueDetailDTO.GetBundleCatalogueDetail>> GetBundleDetailAsync(string name)
        {
            var result = new Result<GetBundleCatalogueDetail>();
            string url = $"{BusinessManager.BaseURL}/catalogue/bundle/{name}";
            try
            {

                var response = await _consumeApi.GetApi<GetBundleCatalogueDetail>(url);

                if (response == null)
                {
                    result.Success = false;
                    result.Data = null;
                    return result;
                }

                result.Success = true;
                result.Data = response;
            }
            catch (Exception) {
                result.Success = false;
                result.Data = null;
            }
            return result;
        }
        #endregion
        #region GetBundles
        public async Task<Result<GetBundleCatalogueResponse>> GetBundlesAsync( string region)
        {
            var result = new Result<GetBundleCatalogueResponse>();

            string url = $"{BusinessManager.BaseURL}/catalogue?page=2&perPage=50&direction=desc&orderBy=speed&region={region}";

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

       
        #endregion
        
        public Result<List<CountriesDTO>> GetCountries()
        {
            var listOfCountry = _db.Countries.Select(a=> new CountriesDTO { CountryName = a.CountryName, Iso2 = a.Iso2, Iso3 = a.Iso3 }).ToList();


            return new Result<List<CountriesDTO>>() {
            
                Data = listOfCountry,
            
            };
        }
    }
}

