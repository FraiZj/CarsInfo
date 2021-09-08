using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.BusinessLogic.Mappers
{
    public class TokenServiceMapper
    {
        public UserRefreshTokenDto MapToUserRefreshTokenDto(UserRefreshToken userRefreshToken)
        {
            return new UserRefreshTokenDto
            {
                Token = userRefreshToken.Token,
                ExpiryTime = userRefreshToken.ExpiryTime,
                UserId = userRefreshToken.UserId
            };
        }
        
        public UserRefreshToken MapToUserRefreshToken(UserRefreshTokenDto userRefreshToken)
        {
            return new UserRefreshToken
            {
                Token = userRefreshToken.Token,
                ExpiryTime = userRefreshToken.ExpiryTime,
                UserId = userRefreshToken.UserId
            };
        }
    }
}