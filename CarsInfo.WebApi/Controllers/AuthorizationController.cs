﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AuthorizationController : ControllerBase
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
            if (authViewModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var accessToken = authViewModel.AccessToken;
            var refreshToken = authViewModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userId = principal.GetUserId();
            var user = await _tokenService.GetUserRefreshTokenAsync(userId.Value);

            if (user == null || user.Token != refreshToken || user.ExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var userRefreshToken = new UserRefreshTokenDto
            {
                UserId = userId.Value,
                Token = newRefreshToken
            };
            await _tokenService.UpdateRefreshTokenByUserIdAsync(userRefreshToken);

            return Ok(new AuthViewModel(newAccessToken, newRefreshToken));
        }

        [Authorize, HttpPost("revoke-token")]
        public async Task<IActionResult> Revoke()
        {
            var user = await _userService.GetByEmailAsync(User.GetEmail());

            if (user == null)
            {
                return BadRequest();
            }

            var userRefreshToken = new UserRefreshTokenDto
            {
                UserId = User.GetUserId().Value,
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

            var userDto = await _userService.GetByEmailAsync(user.Email);
            var userRefreshToken = new UserRefreshTokenDto
            {
                UserId = userDto.Id,
                Token = refreshToken,
                ExpiryTime = DateTimeOffset.Now.AddDays(7)
            };
            await _tokenService.UpdateRefreshTokenByUserIdAsync(userRefreshToken);

            return Ok(new AuthViewModel(accessToken, refreshToken));
        }
    }
}
