using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.Application.BusinessLogic.OperationResult;
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
        private readonly IRoleService _roleService;

        public UserService(
            IUsersRepository usersRepository,
            IGenericRepository<UserRole> userRoleRepository,
            IGenericRepository<Role> roleRepository,
            ILogger<UserService> logger,
            UserServiceMapper mapper,
            IRoleService roleService)
        {
            _usersRepository = usersRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddAsync(UserDto entity)
        {
            try
            {
                var filter = new FilterModel(new FiltrationField("[User].Email", entity.Email));
                var userAlreadyExist = await _usersRepository.ContainsAsync(filter.Filters);
                if (userAlreadyExist ?? true)
                {
                    return OperationResult<int>.FailureResult($"User with email'{entity.Email}' already exists");
                }
                
                var user = _mapper.MapToUser(entity);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var userId = await _usersRepository.AddAsync(user);
                var roleId = await _roleService.GetRoleIdAsync(Roles.User);
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

        public async Task<OperationResult<bool>> ContainsUserWithEmailAsync(string email)
        {
            try
            {
                var filter = new FilterModel(new FiltrationField("[User].Email", email));
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
                return OperationResult<UserDto>.SuccessResult(_mapper.MapToUserDto(user));
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
        
        public async Task<OperationResult> ResetPasswordAsync(string email, string password)
        {
            try
            {
                var user = await _usersRepository.GetByEmailAsync(email);

                if (user is null)
                {
                    return OperationResult.FailureResult($"User with email='{email}' does not exist");
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                await _usersRepository.UpdateAsync(user);

                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while resetting user password");
                return OperationResult.ExceptionResult();
            }
        }
    }
}
