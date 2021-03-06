using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.Application.BusinessLogic.External.Auth.Google;
using CarsInfo.Application.BusinessLogic.External.Auth.Google.Models;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Extensions;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IGenericRepository<UserRole> _userRoleRepository;
        private readonly IRoleService _roleService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly ITokenService _tokenService;
        private readonly UserServiceMapper _mapper;

        public AuthenticationService(
            IUsersRepository usersRepository,
            IGenericRepository<UserRole> userRoleRepository,
            IRoleService roleService,
            IGoogleAuthService googleAuthService,
            ITokenService tokenService,
            UserServiceMapper mapper)
        {
            _usersRepository = usersRepository;
            _userRoleRepository = userRoleRepository;
            _roleService = roleService;
            _googleAuthService = googleAuthService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<OperationResult<ICollection<Claim>>> AuthenticateInternalUserAsync(UserDto entity)
        {
            var user = await _usersRepository.GetByEmailAsync(entity.Email);

            if (user is null || user.IsExternal)
            {
                return OperationResult<ICollection<Claim>>.FailureResult(
                    $"User with email'{entity.Email}' does not exist");
            }

            if (!BCrypt.Net.BCrypt.Verify(entity.Password, user.Password))
            {
                return OperationResult<ICollection<Claim>>.FailureResult(
                    $"Incorrect password for user with email='{entity.Email}'");
            }

            var claims = GetClaims(user);
            return OperationResult<ICollection<Claim>>.SuccessResult(claims);
        }

        public async Task<OperationResult<ICollection<Claim>>> AuthenticateExternalUserAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);

            if (user is null || !user.IsExternal)
            {
                return OperationResult<ICollection<Claim>>.FailureResult(
                    $"User with email '{email}' does not exist");
            }

            var claims = GetClaims(user);
            return OperationResult<ICollection<Claim>>.SuccessResult(claims);
        }

        public async Task<OperationResult<UserDto>> LoginWithGoogleAsync(string token)
        {
            var operation = await _googleAuthService.AuthenticateAsync(token);

            if (!operation.Success)
            {
                return OperationResult<UserDto>.FailureResult(operation.FailureMessage);
            }

            var user = await _usersRepository.GetByEmailAsync(operation.Result.Email);

            if (user is null)
            {
                await CreateUserFromGoogle(operation.Result);
                user = await _usersRepository.GetByEmailAsync(operation.Result.Email);
            }

            return OperationResult<UserDto>.SuccessResult(_mapper.MapToUserDto(user));
        }

        public async Task<OperationResult<bool>> VerifyEmailAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);

            if (user is null)
            {
                return OperationResult<bool>.FailureResult($"User with email='{email}' does not exist");
            }

            if (user.EmailVerified)
            {
                return OperationResult<bool>.SuccessResult(true);
            }

            user.EmailVerified = true;
            await _usersRepository.UpdateAsync(user);

            return OperationResult<bool>.SuccessResult(true);
        }

        public async Task<OperationResult<ICollection<Claim>>> GetUserClaimsByTokensAsync(
            string accessToken, string refreshToken)
        {
            var getPrincipalOperation = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            if (!getPrincipalOperation.Success)
            {
                return OperationResult<ICollection<Claim>>.FailureResult(getPrincipalOperation.FailureMessage);
            }

            var principal = getPrincipalOperation.Result;
            var userId = principal.GetUserId();

            if (!userId.HasValue)
            {
                return OperationResult<ICollection<Claim>>.FailureResult("Cannot identify user");
            }

            if (await ValidateRefreshToken(userId.Value, refreshToken) == false)
            {
                return OperationResult<ICollection<Claim>>.FailureResult("Invalid refresh token");
            }

            var user = await _usersRepository.GetByEmailAsync(principal.GetEmail());

            return OperationResult<ICollection<Claim>>.SuccessResult(GetClaims(user));
        }

        private async Task<bool> ValidateRefreshToken(int userId, string refreshToken)
        {
            var getRefreshTokenOperation = await _tokenService.GetUserRefreshTokenAsync(userId);
            return getRefreshTokenOperation.Success && 
                   TokenService.IsTokenValid(getRefreshTokenOperation.Result, refreshToken);
        }
        
        private async Task CreateUserFromGoogle(GoogleAuthResult googleAuth)
        {
            var userId = await _usersRepository.AddAsync(new User
            {
                FirstName = googleAuth.FirstName,
                LastName = googleAuth.LastName,
                Email = googleAuth.Email,
                IsExternal = true,
                EmailVerified = true
            });
            var roleId = await _roleService.GetRoleIdAsync(Roles.User);
            await _userRoleRepository.AddAsync(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
        }

        private static ICollection<Claim> GetClaims(User user)
        {
            var claims = user.Roles.Select(
                userRole => new Claim(ClaimTypes.Role, userRole.Name)).ToList();
            claims.AddRange(new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ApplicationClaims.Id, user.Id.ToString()),
            });

            if (user.EmailVerified)
            {
                claims.Add(new Claim(ApplicationClaims.EmailVerified, string.Empty));
            }

            return claims;
        }
    }
}