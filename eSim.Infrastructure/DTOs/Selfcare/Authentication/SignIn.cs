﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Selfcare.Authentication
{
    public class SignIn
    {
        [Required (ErrorMessage ="Username is required")]
        public string Email { get; set; }

        [Required (ErrorMessage ="Password is required")]
        public string Password { get; set; }

        
    }
}
