using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _usersRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(UserDto entity)
        {
            try
            {
                var user = _mapper.Map<User>(entity);
                await _usersRepository.AddAsync(user);
            }
            catch (Exception ex)
            {
            }
        }

        public Task<IEnumerable<Claim>> AuthorizeAsync(UserDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                await _usersRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateAsync(UserDto entity)
        {
            try
            {
                var user = _mapper.Map<User>(entity);
                await _usersRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
