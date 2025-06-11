using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Bundle
{
    public class RegionDTORequest
    {
        [Required(ErrorMessage = "Region is required.")]
        public string Region { get; set; }

        // [Required(ErrorMessage = "Countries are required.")]
        public string? Countries { get; set; } 

        public int Page { get; set; } = 0;

        public int PerPage { get; set; } = 10;

        //move hard coded values to static class

        public string Direction { get; set; } = "desc";

        //create enum of speed values
        public string OrderBy { get; set; } = "speed";

    }
}
