﻿using System;
using System.Collections.Generic;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }

        public ICollection<CarDto> FavoriteCars { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
