using System;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class UserRefreshTokenDto
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }
    }
}