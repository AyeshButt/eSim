using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{

    [Table("CountriesData", Schema = "ref")]
    public class Countries
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string Iso3 { get; set; }
        public string Iso2 { get; set; }
        public int RegionId {  get; set; }
    }
}
