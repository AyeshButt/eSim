﻿using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs.Account
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public  string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }


}
