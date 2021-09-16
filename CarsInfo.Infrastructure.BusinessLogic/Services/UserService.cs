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
using CarsInfo.Application.BusinessLogic.Validators;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IGenericRepository<UserRole> _userRoleRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly ILogger<UserService> _logger;
        private readonly UserServiceMapper _mapper;
        private readonly IGoogleAuthService _googleAuthService;

        public UserService(
            IUsersRepository usersRepository,
            IGenericRepository<UserRole> userRoleRepository,
            IGenericRepository<Role> roleRepository,
            ILogger<UserService> logger,
            UserServiceMapper mapper,
            IGoogleAuthService googleAuthService)
        {
            _usersRepository = usersRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _googleAuthService = googleAuthService;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddAsync(UserDto entity)
        {
            try
            {
                var filter = new FilterModel(new FiltrationField("User.Email", entity.Email));
                var userAlreadyExist = await _usersRepository.ContainsAsync(filter.Filters);
                if (userAlreadyExist ?? true)
                {
                    return OperationResult<int>.FailureResult($"User with email'{entity.Email}' already exists");
                }
                
                var user = _mapper.MapToUser(entity);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var userId = await _usersRepository.AddAsync(user);
                var roleId = await GetRoleIdAsync(Roles.User);
                await _userRoleRepository.AddAsync(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });
                
                return OperationResult<int>.SuccessResult(userId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating user");
                return OperationResult<int>.ExceptionResult();
            }
        }
        
        private async Task<int> GetRoleIdAsync(string roleName)
        {
            var role = await _roleRepository.GetAsync(new List<FiltrationField>
            {
                new("Name", roleName)
            });

            ValidationHelper.ThrowIfNull(role);
            
            return role.Id;
        }

        public async Task<OperationResult<ICollection<Claim>>> GetExternalUserClaimsAsync(string email)
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

        public async Task<OperationResult<ICollection<Claim>>> GetInternalUserClaimsAsync(UserDto entity)
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

        private static ICollection<Claim> GetClaims(User user)
        {
            var claims = user.Roles.Select(
                userRole => new Claim(ClaimTypes.Role, userRole.Name)).ToList();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim("Id", user.Id.ToString()));

            return claims;
        }

        public async Task<OperationResult<bool>> ContainsUserWithEmailAsync(string email)
        {
            try
            {
                var filter = new FilterModel(new FiltrationField("User.Email", email));
                var contains = await _usersRepository.ContainsAsync(filter.Filters);

                if (contains is null)
                {
                    return OperationResult<bool>.FailureResult("Cannot fetch result");
                }
                
                return OperationResult<bool>.SuccessResult(contains.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching user with email={email}");
                return OperationResult<bool>.ExceptionResult();
            }
        }

        public async Task<OperationResult<UserDto>> LoginWithGoogle(string token)
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
            var roleId = await GetRoleIdAsync(Roles.User);
            await _userRoleRepository.AddAsync(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
        }

        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            try
            {
                var user = await _usersRepository.GetAsync(id);
                if (user is null)
                {
                    return OperationResult.FailureResult($"User with id={id} does not exist");
                }
                await _usersRepository.DeleteAsync(id);
                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while deleting user with id={id}");
                return OperationResult.ExceptionResult();
            }
        }

        public async Task<OperationResult<IEnumerable<UserDto>>> GetAllAsync()
        {
            try
            {
                var users = await _usersRepository.GetAllAsync();
                return OperationResult<IEnumerable<UserDto>>.SuccessResult(_mapper.MapToUsersDtos(users));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching users");
                return OperationResult<IEnumerable<UserDto>>.ExceptionResult();
            }
        }

        public async Task<OperationResult<UserDto>> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _usersRepository.GetByEmailAsync(email);
                return user is null ? 
                    OperationResult<UserDto>.FailureResult($"User with email='{email}' does not exist") : 
                    OperationResult<UserDto>.SuccessResult(_mapper.MapToUserDto(user));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching user with email={email}");
                return OperationResult<UserDto>.ExceptionResult();
            }
        }

        public async Task<OperationResult> UpdateAsync(UserDto entity)
        {
            try
            {
                var user = await _usersRepository.GetAsync(entity.Id);
                if (user is null)
                {
                    return OperationResult.FailureResult($"User with id={entity.Id} does not exist");
                }
                await _usersRepository.UpdateAsync(_mapper.MapToUser(entity));
                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while updating user with id={entity.Id}");
                return OperationResult.ExceptionResult();
            }
        }
    }
}
