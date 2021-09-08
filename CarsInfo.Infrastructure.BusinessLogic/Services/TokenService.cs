using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.AuthModels;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Validators;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;
        private readonly ILogger<TokenService> _logger;
        private readonly TokenServiceMapper _mapper;
        private readonly ApiAuthSetting _authSetting;

        public TokenService(
            IOptions<ApiAuthSetting> authSetting,
            IGenericRepository<UserRefreshToken> userRefreshTokenRepository,
            ILogger<TokenService> logger,
            TokenServiceMapper mapper)
        {
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _logger = logger;
            _mapper = mapper;
            _authSetting = authSetting.Value;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSetting.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                _authSetting.Issuer,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromSeconds(_authSetting.ExpirationTime)),
                signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(jwt);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSetting.Secret)),
                ValidIssuer = _authSetting.Issuer,
                ValidateIssuer = true,
                ValidateAudience = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                    StringComparison.InvariantCultureIgnoreCase))
            {

                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<UserRefreshTokenDto> GetUserRefreshTokenAsync(int userId)
        {
            var userRefreshToken = await _userRefreshTokenRepository.GetAsync(userId);
            return _mapper.MapToUserRefreshTokenDto(userRefreshToken);
        }
        
        public async Task UpdateRefreshTokenByUserIdAsync(UserRefreshTokenDto userRefreshTokenDto)
        {
            try
            {
                ValidationHelper.ThrowIfNull(userRefreshTokenDto);
                ValidationHelper.ThrowIfStringNullOrWhiteSpace(userRefreshTokenDto.Token);

                var filters = new List<FiltrationField>
                {
                    new ("UserId", userRefreshTokenDto.UserId)
                };
                var userRefreshToken = await _userRefreshTokenRepository.GetAsync(filters);

                if (userRefreshToken is null)
                {
                    await AddRefreshTokenAsync(userRefreshTokenDto);
                    return;
                }
                
                userRefreshToken.Token = userRefreshTokenDto.Token;

                if (userRefreshTokenDto.ExpiryTime is not null)
                {
                    userRefreshToken.ExpiryTime = userRefreshTokenDto.ExpiryTime;
                }

                await _userRefreshTokenRepository.UpdateAsync(userRefreshToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while updating refresh token");
            }
        }

        private Task AddRefreshTokenAsync(UserRefreshTokenDto userRefreshTokenDto)
        {
            var token = _mapper.MapToUserRefreshToken(userRefreshTokenDto);
            return _userRefreshTokenRepository.AddAsync(token);
        }
    }
}
