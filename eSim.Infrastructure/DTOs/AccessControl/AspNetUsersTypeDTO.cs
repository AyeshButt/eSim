﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.AccessControl
{
    public class AspNetUsersTypeDTO
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
    }
}
