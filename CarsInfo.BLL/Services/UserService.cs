using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.BLL.Assistance;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Mappers;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.BLL.Models.Enums;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace CarsInfo.BLL.Services
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

            var role = await _roleRepository.GetAsync(new List<FilterModel>
            {
                new("Name", Roles.User)
            });

            ValidationHelper.ThrowIfNull(role);

            return role.Id;
        }

        public async Task<ICollection<Claim>> AuthorizeAsync(UserDto entity)
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
