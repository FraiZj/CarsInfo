using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        
        string GenerateRefreshToken();
        
        OperationResult<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);

        Task<OperationResult<UserRefreshTokenDto>> GetUserRefreshTokenAsync(int userId);
        
        Task<OperationResult.OperationResult> UpdateRefreshTokenByUserIdAsync(UserRefreshTokenDto userRefreshTokenDto);
    }
}
