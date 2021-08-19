using System.Collections.Generic;
using System.Security.Claims;

namespace CarsInfo.WebApi.StartupConfiguration.Authentication.Factories
{
    public interface ITokenFactory
    {
        string CreateToken(ICollection<Claim> claims);
    }
}
