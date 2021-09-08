using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels;
using CarsInfo.WebApi.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AuthorizationController : AppControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthorizationControllerMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthorizationController(
            IUserService userService,
            AuthorizationControllerMapper mapper,
            ITokenService tokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var contains = await _userService.ContainsUserWithEmailAsync(model.Email);

            if (contains ?? true)
            {
                return BadRequest($"Email '{model.Email}' is already in use");
            }
            
            var user = _mapper.MapToUserDto(model);
            await _userService.AddAsync(user);
            return await AuthorizeAsync(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = _mapper.MapToUserDto(model);
            return await AuthorizeAsync(user);
        }

        [HttpGet("emailAvailable/{email}")]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            var contains = await _userService.ContainsUserWithEmailAsync(email);

            if (contains is null)
            {
                return BadRequest("Could not fetch the result.");
            }

            return Ok(!contains);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] AuthViewModel authViewModel)
        {
            var getPrincipalOperation = _tokenService.GetPrincipalFromExpiredToken(authViewModel.AccessToken);

            if (!getPrincipalOperation.Success)
            {
                return getPrincipalOperation.IsException ?
                    ApplicationError() :
                    BadRequest(getPrincipalOperation.FailureMessage);
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
                return getRefreshTokenOperation.IsException ?
                    ApplicationError() :
                    BadRequest(getRefreshTokenOperation.FailureMessage);
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
                return updateRefreshTokenOperation.IsException ?
                    ApplicationError() :
                    BadRequest(updateRefreshTokenOperation.FailureMessage);
            }
            
            return Ok(new AuthViewModel(_tokenService.GenerateAccessToken(principal.Claims), newRefreshToken));
        }

        [NonAction]
        private bool IsTokenValid(UserRefreshTokenDto userRefreshTokenDto, string refreshToken)
        {
            return userRefreshTokenDto == null || userRefreshTokenDto.Token != refreshToken ||
                   userRefreshTokenDto.ExpiryTime <= DateTime.Now;
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
            await _tokenService.UpdateRefreshTokenByUserIdAsync(userRefreshToken);

            return Ok();
        }

        [NonAction]
        private async Task<IActionResult> AuthorizeAsync(UserDto user)
        {
            var claims = await _userService.GetUserClaimsAsync(user);

            if (!claims.Any())
            {
                return BadRequest("Cannot authorize user");
            }
            
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
;
            var userRefreshToken = new UserRefreshTokenDto
            {
                UserId = Convert.ToInt32(claims.First(c => c.Type == "Id").Value),
                Token = refreshToken,
                ExpiryTime = DateTimeOffset.Now.AddDays(7)
            };
            await _tokenService.UpdateRefreshTokenByUserIdAsync(userRefreshToken);

            return Ok(new AuthViewModel(accessToken, refreshToken));
        }
    }
}
