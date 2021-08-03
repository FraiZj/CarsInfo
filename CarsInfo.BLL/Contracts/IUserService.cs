﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.BLL.Models.Dtos;

namespace CarsInfo.BLL.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto> GetByIdAsync(int id);
        
        Task AddAsync(UserDto entity);

        Task UpdateAsync(UserDto entity);

        Task DeleteByIdAsync(int id);

        Task<ICollection<Claim>> AuthorizeAsync(UserDto entity);
    }
}
