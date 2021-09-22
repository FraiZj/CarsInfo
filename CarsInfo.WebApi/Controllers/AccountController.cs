using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AccountController : AppController
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet("email/verify")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string email)
        {
            var operation = await _authenticationService.VerifyEmailAsync(email);
            return operation.Success ? 
                Ok() : 
                BadRequest(operation.FailureMessage);
        }
    }
}