using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IAuthenticationService
    {
        Task<OperationResult.OperationResult<ICollection<Claim>>> AuthenticateInternalUserAsync(UserDto entity);

        Task<OperationResult.OperationResult<ICollection<Claim>>> AuthenticateExternalUserAsync(string email);

        Task<OperationResult.OperationResult<UserDto>> LoginWithGoogleAsync(string token);
        
        Task<OperationResult.OperationResult<bool>> VerifyEmailAsync(string email);

        Task<OperationResult.OperationResult<ICollection<Claim>>> GetUserClaimsByTokensAsync(
            string accessToken, string refreshToken);

        Task<OperationResult.OperationResult> ResetPasswordAsync(string email, string password);
    }
}
