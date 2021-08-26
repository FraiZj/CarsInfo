using System.Collections.Generic;
using System.Security.Claims;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
