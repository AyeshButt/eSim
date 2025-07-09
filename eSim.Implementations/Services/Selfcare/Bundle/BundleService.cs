using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Bundle;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.DTOs.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.Selfcare.Refrence;
using static eSim.Infrastructure.DTOs.Middleware.Bundle.GetBundleCatalogueDetailDTO;

namespace eSim.Implementations.Services.Selfcare.Bundle
{
    public class BundleService(IMiddlewareConsumeApi consumeApi, ICountyService countryService) : IBundleService
    {
        private readonly IMiddlewareConsumeApi _consumeApi = consumeApi;
        private readonly ICountyService _countryService = countryService;

        public async Task<Result<BundleViewModel>> GetBundles()
        {
            Result<BundleViewModel> result = new()
            {
                Data = new BundleViewModel()
            };

            BundleRequest dto = new();

            var Url = BusinessManager.MdwBaseURL + BusinessManager.BundelRegion;

            result.Data.CountriesDTO = await _countryService.Countries();

            result.Data.Regions = await _countryService.RegionsAsync();

            var response = await _consumeApi.Post<GetBundleCatalogueResponse, BundleRequest>(Url, dto);

            result.Data.BundlesDto = response.Data;

            return result;
        }


        public async Task<Result<GetBundleCatalogueDetailsResponse>> BundleDetail(string dto)
        {
            var Url = BusinessManager.MdwBaseURL + BusinessManager.Bundeldetail;

            //var fullUrl = $"{Url}?name={HttpUtility.UrlEncode(dto)}";
            var FullURl = $"{Url}{Uri.EscapeDataString(dto)}";

            var response = await _consumeApi.Get<GetBundleCatalogueDetailsResponse>(FullURl);

            return response;
        }

        public async Task<Result<CreateOrderResponse>> CreateOrderAsync(OrderModalViewModel input)
        {
            var model = new CreateOrderRequest();

            model.Order = new List<CreateOrderDetailDTO>()
            {
               new CreateOrderDetailDTO()
               {
                   Type = BusinessManager.OrderBundleType,
                   Item = input.BundleName,
                   Quantity = input.Quantity,
               }
            };

            var url = $"{BusinessManager.MiddlewareBaseURL}/orders";

            var response = await _consumeApi.Post<CreateOrderResponse, CreateOrderRequest>(url, model);

            return response;

        }

        
    }
}
