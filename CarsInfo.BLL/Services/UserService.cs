using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CarsInfo.BLL.Assistance;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace CarsInfo.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUsersRepository usersRepository, 
            IMapper mapper, 
            ILogger<UserService> logger)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(UserDto entity)
        {
            try
            {
                ValidateUserDto(entity);
                var user = _mapper.Map<User>(entity);
                await _usersRepository.AddAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating user");
            }
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

                return claims;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while authorizing user with email={entity.Email}");
                return new List<Claim>();
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
                var usersDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return usersDtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching users");
                return new List<UserDto>();
            }
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            try
            {
                var user = await _usersRepository.GetAllAsync();
                var userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching user with id={id}");
                return null;
            }
        }

        public async Task UpdateAsync(UserDto entity)
        {
            try
            {
                ValidateUserDto(entity);
                var user = _mapper.Map<User>(entity);
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
