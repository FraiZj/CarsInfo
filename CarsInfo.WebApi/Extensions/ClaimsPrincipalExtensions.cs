using System.Linq;
using System.Security.Claims;

namespace CarsInfo.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            return int.TryParse(principal?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var id) ?
                id :
                null;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}
