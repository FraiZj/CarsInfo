using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Commands;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Models;
using CarsInfo.Infrastructure.BusinessLogic.Extensions;
using CarsInfo.WebApi.Attributes;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.ViewModels.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AccountController : AppController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public AccountController(
            IAuthenticationService authenticationService,
            ITokenService tokenService,
            IUserService userService,
            IMediator mediator)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _userService = userService;
            _mediator = mediator;
        }

        [HttpPost("verify-email"), VerifyToken]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var jwt = _tokenService.DecodeJwtToken(token);
            var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
            var operation = await _authenticationService.VerifyEmailAsync(email);
            return operation.Success 
                ?  Ok() 
                : BadRequest(operation.FailureMessage);
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

            if (getUserOperation.Result is null)
            {
                return NotFound();
            }
            
            var sendEmailVerificationOperation = await _mediator.Send(new SendEmailVerificationCommand
            {
                EmailBodyModel = new EmailBodyModel
                {
                    Email = email,
                    FirstName = getUserOperation.Result.FirstName,
                    LastName = getUserOperation.Result.LastName
                }
            });
            
            return sendEmailVerificationOperation.Success
                ? Ok()
                : BadRequest(sendEmailVerificationOperation.FailureMessage);
        }
        
        [HttpPost("send-reset-password-email")]
        public async Task<IActionResult> SendResetPasswordEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Cannot identify user");
            }

            var getUserOperation = await _userService.GetByEmailAsync(email);

            if (!getUserOperation.Success)
            {
                return BadRequest(getUserOperation.FailureMessage);
            }

            if (getUserOperation.Result is null)
            {
                return NotFound();
            }
            
            var sendEmailVerificationOperation = await _mediator.Send(new SendResetPasswordEmailCommand
            {
                EmailBodyModel = new EmailBodyModel
                {
                    Email = email,
                    FirstName = getUserOperation.Result.FirstName,
                    LastName = getUserOperation.Result.LastName
                }
            });

            return sendEmailVerificationOperation.Success
                ? Ok()
                : BadRequest(sendEmailVerificationOperation.FailureMessage);
        }
        
        [HttpPost("reset-password"), VerifyToken]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordPayload payload,
            [FromQuery] string token)
        {
            var jwt = _tokenService.DecodeJwtToken(token);
            var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
            var operation = await _userService.ResetPasswordAsync(email, payload.Password);
            return operation.Success 
                ? Ok() 
                : BadRequest(operation.FailureMessage);
        }
    }
}