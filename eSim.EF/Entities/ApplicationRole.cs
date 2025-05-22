using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Entities
{
    public class ApplicationRole : IdentityRole
    {
        [Required]
        public string CreatedBy { get; set; }
    }
}
