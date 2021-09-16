using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.External.Auth.Google.Models;

namespace CarsInfo.Application.BusinessLogic.External.Auth.Google
{
    public interface IGoogleAuthService
    {
        Task<OperationResult.OperationResult<GoogleAuthResult>> AuthenticateAsync(string accessToken);
    }
}
