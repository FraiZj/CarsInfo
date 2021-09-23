using System.Linq;
using System.Security.Claims;
using CarsInfo.Application.BusinessLogic.Enums;

namespace CarsInfo.Infrastructure.BusinessLogic.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            var idClaim = principal?.Claims.FirstOrDefault(c => c.Type == ApplicationClaims.Id);
            return int.TryParse(idClaim?.Value, out var id) ?
                id :
                null;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}
