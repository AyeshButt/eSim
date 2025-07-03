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
using Microsoft.AspNetCore.Http;
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
                result = await _consumeApi.GetApii<GetBundleCatalogueDetailsResponse>(url);
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
        public async Task<Result<GetBundleCatalogueResponse>> GetBundlesAsync(BundleRequest request)
        {
            var result = new Result<GetBundleCatalogueResponse>();
   

            var url = $"{BusinessManager.BaseURL}/catalogue?page={request.Page}&perPage={request.PerPage}&direction={request.Direction}&orderBy={request.OrderBy}&region={request.Region}";



            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                url += $"&countries={request.Countries}";   
            }

            try
            {
                result = await _consumeApi.GetApii<GetBundleCatalogueResponse>(url);

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

