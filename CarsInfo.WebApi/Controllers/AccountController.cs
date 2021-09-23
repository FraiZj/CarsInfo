using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Infrastructure.BusinessLogic.Extensions;
using CarsInfo.WebApi.Account;
using CarsInfo.WebApi.Account.Attributes;
using CarsInfo.WebApi.Account.Models;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AccountController : AppController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public AccountController(
            IAuthenticationService authenticationService,
            ITokenService tokenService,
            IAccountService accountService,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _accountService = accountService;
            _userService = userService;
        }

        [HttpGet("verify-email"), VerifyToken]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var jwt = _tokenService.DecodeJwtToken(token);
            var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
            var operation = await _authenticationService.VerifyEmailAsync(email);
            return operation.Success ? Ok() : BadRequest(operation.FailureMessage);
        }

        [HttpPost("send-verification-email"), Authorize]
        public async Task<IActionResult> SendVerificationEmail()
        {
            var email = User.GetEmail();

            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Cannot identify user");
            }

            var getUserOperation = await _userService.GetByEmailAsync(email);

            if (!getUserOperation.Success)
            {
                return BadRequest(getUserOperation.FailureMessage);
            }

            var sendEmailVerificationOperation = await _accountService.SendEmailVerificationAsync(
                new EmailVerificationModel
                {
                    Email = email,
                    FirstName = getUserOperation.Result.FirstName,
                    LastName = getUserOperation.Result.LastName
                });

            return sendEmailVerificationOperation.Success
                ? Ok()
                : BadRequest(sendEmailVerificationOperation.FailureMessage);
        }
    }
}