using System.Collections.Generic;
using System.Security.Claims;

namespace CarsInfo.WebApi.Authorization
{
    public interface ITokenFactory
    {
        string CreateToken(ICollection<Claim> claims);
    }
}
