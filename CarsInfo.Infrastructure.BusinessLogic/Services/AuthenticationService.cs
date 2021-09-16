using System;
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
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IGenericRepository<UserRole> _userRoleRepository;
        private readonly IRoleService _roleService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly UserServiceMapper _mapper;

        public AuthenticationService(
            IUsersRepository usersRepository,
            IGenericRepository<UserRole> userRoleRepository,
            IRoleService roleService,
            IGoogleAuthService googleAuthService,
            ILogger<AuthenticationService> logger,
            UserServiceMapper mapper)
        {
            _usersRepository = usersRepository;
            _userRoleRepository = userRoleRepository;
            _roleService = roleService;
            _googleAuthService = googleAuthService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OperationResult<ICollection<Claim>>> AuthenticateInternalUserAsync(UserDto entity)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while authorizing user with email={entity.Email}");
                return OperationResult<ICollection<Claim>>.ExceptionResult();
            }
        }

        public async Task<OperationResult<ICollection<Claim>>> AuthenticateExternalUserAsync(string email)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while authorizing user with email={email}");
                return OperationResult<ICollection<Claim>>.ExceptionResult();
            }
        }

        public async Task<OperationResult<UserDto>> LoginWithGoogleAsync(string token)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while user authentication with google");
                return OperationResult<UserDto>.ExceptionResult();
            }
        }

        private async Task CreateUserFromGoogle(GoogleAuthResult googleAuth)
        {
            var userId = await _usersRepository.AddAsync(new User
            {
                FirstName = googleAuth.FirstName,
                LastName = googleAuth.LastName,
                Email = googleAuth.Email,
                IsExternal = true
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
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim("Id", user.Id.ToString()));

            return claims;
        }
    }
}
