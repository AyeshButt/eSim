using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Middleware.Bundle
{
    public class BundleNameDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}
