using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.Selfcare.Refrence
{
    public interface ICountyService
    {
        public Task<List<CountriesDTORequest>> Countries();
        public Task<List<RegionsResponseDTO>> RegionsAsync();

    }
}
