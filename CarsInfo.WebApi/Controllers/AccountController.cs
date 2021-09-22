using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.WebApi.Account.Attributes;
using CarsInfo.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AccountController : AppController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public AccountController(
            IAuthenticationService authenticationService,
            ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        [HttpGet("email/verify"), VerifyToken]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var jwt = _tokenService.DecodeJwtToken(token);
            var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
            var operation = await _authenticationService.VerifyEmailAsync(email);
            return operation.Success ? 
                Ok() : 
                BadRequest(operation.FailureMessage);
        }
    }
}