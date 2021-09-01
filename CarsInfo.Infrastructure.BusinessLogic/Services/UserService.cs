using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
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

        public UserService(
            IUsersRepository usersRepository,
            IGenericRepository<UserRole> userRoleRepository,
            IGenericRepository<Role> roleRepository,
            ILogger<UserService> logger,
            UserServiceMapper mapper)
        {
            _usersRepository = usersRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(UserDto entity)
        {
            try
            {
                ValidateUserDto(entity);
                var user = _mapper.MapToUser(entity);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var userId = await _usersRepository.AddAsync(user);
                var roleId = await GetRoleIdAsync(Roles.User);
                await _userRoleRepository.AddAsync(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating user");
            }
        }
        
        private async Task<int> GetRoleIdAsync(string roleName)
        {
            ValidationHelper.ThrowIfStringNullOrWhiteSpace(roleName);

            var role = await _roleRepository.GetAsync(new List<FiltrationField>
            {
                new("Name", Roles.User)
            });

            ValidationHelper.ThrowIfNull(role);

            return role.Id;
        }

        public async Task<ICollection<Claim>> GetUserClaimsAsync(UserDto entity)
        {
            try
            {
                ValidationHelper.ThrowIfNull(entity);
                ValidationHelper.ThrowIfStringNullOrWhiteSpace(entity.Email);
                ValidationHelper.ThrowIfStringNullOrWhiteSpace(entity.Password);

                var user = await _usersRepository.GetWithRolesAsync(entity.Email);

                if (user is null)
                {
                    return new List<Claim>();
                }

                if (!BCrypt.Net.BCrypt.Verify(entity.Password, user.Password))
                {
                    return new List<Claim>();
                }

                var claims = user.Roles.Select(
                    userRole => new Claim(ClaimTypes.Role, userRole.Name)).ToList();
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim("Id", user.Id.ToString()));

                return claims;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while authorizing user with email={entity.Email}");
                return new List<Claim>();
            }
        }

        public async Task<bool?> ContainsUserWithEmailAsync(string email)
        {
            try
            {
                var user = await _usersRepository.GetWithRolesAsync(email);
                return user != null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching user with email={email}");
                return null;
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                await _usersRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while deleting user with id={id}");
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            try
            {
                var users = await _usersRepository.GetAllAsync();
                var usersDtos = _mapper.MapToUsersDtos(users);
                return usersDtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching users");
                return new List<UserDto>();
            }
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _usersRepository.GetWithRolesAsync(email);
                var userDto = _mapper.MapToUserDto(user);
                return userDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching user with email={email}");
                return null;
            }
        }

        public async Task UpdateAsync(UserDto entity)
        {
            try
            {
                ValidateUserDto(entity);
                var user = _mapper.MapToUser(entity);
                await _usersRepository.UpdateAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while updating user with id={entity.Id}");
            }
        }
        
        public async Task UpdateRefreshTokenByEmailAsync(
            string email, 
            string refreshToken, 
            DateTimeOffset? refreshTokenExpiryTime = null)
        {
            try
            {
                var user = await _usersRepository.GetWithRolesAsync(email);
                user.RefreshToken = refreshToken;

                if (refreshTokenExpiryTime is not null)
                {
                    user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                }

                await _usersRepository.UpdateAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while updating user with email={email}");
            }
        }

        private static void ValidateUserDto(UserDto user)
        {
            ValidationHelper.ThrowIfNull(user);
            ValidationHelper.ThrowIfStringNullOrWhiteSpace(user.Email);
            ValidationHelper.ThrowIfStringNullOrWhiteSpace(user.FirstName);
            ValidationHelper.ThrowIfStringNullOrWhiteSpace(user.LastName);
            ValidationHelper.ThrowIfStringNullOrWhiteSpace(user.Password);
        }
    }
}
