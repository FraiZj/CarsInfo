using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        
        string GenerateRefreshToken();
        
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Task<UserRefreshTokenDto> GetUserRefreshTokenAsync(int userId);
        
        Task UpdateRefreshTokenByUserIdAsync(UserRefreshTokenDto userRefreshTokenDto);
    }
}
