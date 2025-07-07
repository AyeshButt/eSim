using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Middleware.Bundle;

namespace eSim.Infrastructure.DTOs.Selfcare.Bundles
{
    public class BundleViewModel
    {
        public BundleViewModel()
        {
            CountriesDTO = new List<CountriesDTORequest>();
            BundlesDto = new GetBundleCatalogueResponse();
            Regions  = new List<RegionsResponseDTO>();
        }
        
        public List<CountriesDTORequest> CountriesDTO {  get; set; }
        public GetBundleCatalogueResponse BundlesDto {  get; set; }
        public List<RegionsResponseDTO> Regions {  get; set; }
    }
}
