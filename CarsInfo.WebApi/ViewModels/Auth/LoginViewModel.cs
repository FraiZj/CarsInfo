﻿using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
