using System;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AuthorizationController : AppController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly AuthorizationControllerMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthorizationController(
            IAuthenticationService authenticationService,
            IUserService userService,
            AuthorizationControllerMapper mapper,
            ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var containsOperation = await _userService.ContainsUserWithEmailAsync(model.Email);

            if (!containsOperation.Success)
            {
                return BadRequest(containsOperation.FailureMessage);
            }
            
            var user = _mapper.MapToUserDto(model);
            var registerUserResult = await _userService.AddAsync(user);
            
            if (!registerUserResult.Success)
            {
                return BadRequest(containsOperation.FailureMessage);
            }
            
            return await AuthorizeAsync(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = _mapper.MapToUserDto(model);
            return await AuthorizeAsync(user);
        }

        [HttpPost("login/google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleAuthViewModel googleAuth)
        {
            var operation = await _authenticationService.LoginWithGoogleAsync(googleAuth.Token);
            return operation.Success ?
                await AuthorizeAsync(operation.Result, true) :
                BadRequest(operation.FailureMessage);
        }

        [HttpGet("emailAvailable/{email}")]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            var operation = await _userService.ContainsUserWithEmailAsync(email);

            return operation.Success ? 
                Ok(operation.Result) : 
                BadRequest(operation.FailureMessage);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] AuthViewModel authViewModel)
        {
            var getPrincipalOperation = _tokenService.GetPrincipalFromExpiredToken(authViewModel.AccessToken);

            if (!getPrincipalOperation.Success)
            {
                return BadRequest(getPrincipalOperation.FailureMessage);
            }

            var principal = getPrincipalOperation.Result;
            var userId = principal.GetUserId();

            if (!userId.HasValue)
            {
                return BadRequest("Cannot authenticate user");
            }
            
            var getRefreshTokenOperation = await _tokenService.GetUserRefreshTokenAsync(userId.Value);

            if (!getRefreshTokenOperation.Success)
            {
                return BadRequest(getRefreshTokenOperation.FailureMessage);
            }

            if (!IsTokenValid(getRefreshTokenOperation.Result, authViewModel.RefreshToken))
            {
                return BadRequest("Invalid refresh token");
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var userRefreshToken = new UserRefreshTokenDto
            {
                UserId = userId.Value,
                Token = newRefreshToken
            };
            var updateRefreshTokenOperation = await _tokenService.UpdateRefreshTokenByUserIdAsync(userRefreshToken);

            if (!updateRefreshTokenOperation.Success)
            {
                return BadRequest(updateRefreshTokenOperation.FailureMessage);
            }
            
            return Ok(new AuthViewModel(_tokenService.GenerateAccessToken(principal.Claims), newRefreshToken));
        }

        [NonAction]
        private bool IsTokenValid(UserRefreshTokenDto userRefreshTokenDto, string refreshToken)
        {
            return userRefreshTokenDto is not null && userRefreshTokenDto.Token == refreshToken &&
                   userRefreshTokenDto.ExpiryTime > DateTime.Now;
        }
        
        [Authorize, HttpPost("revoke-token")]
        public async Task<IActionResult> Revoke()
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return BadRequest("Cannot authenticate user");
            }
            
            var userRefreshToken = new UserRefreshTokenDto
            {
                UserId = userId.Value,
                Token = null
            };
            var operation = await _tokenService.UpdateRefreshTokenByUserIdAsync(userRefreshToken);

            return operation.Success ?
                Ok("Token revoked") :
                BadRequest(operation.FailureMessage);
        }

        [NonAction]
        private async Task<IActionResult> AuthorizeAsync(UserDto user, bool externalUser = false)
        {
            var getClaimsOperation = externalUser ?
                await _authenticationService.AuthenticateExternalUserAsync(user.Email) :
                await _authenticationService.AuthenticateInternalUserAsync(user);

            if (!getClaimsOperation.Success)
            {
                return BadRequest(getClaimsOperation.FailureMessage);
            }

            var claims = getClaimsOperation.Result;
            var refreshToken = _tokenService.GenerateRefreshToken();
;
            var userRefreshToken = new UserRefreshTokenDto
            {
                UserId = Convert.ToInt32(claims.First(c => c.Type == "Id").Value),
                Token = refreshToken,
                ExpiryTime = DateTimeOffset.Now.AddDays(7)
            };
            var updateRefreshTokenOperation = await _tokenService.UpdateRefreshTokenByUserIdAsync(userRefreshToken);

            if (!updateRefreshTokenOperation.Success)
            {
                return BadRequest(updateRefreshTokenOperation.FailureMessage);
            }
            
            return Ok(new AuthViewModel(_tokenService.GenerateAccessToken(claims), refreshToken));
        }
    }
}
