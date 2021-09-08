using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto> GetByEmailAsync(string email);
        
        Task AddAsync(UserDto entity);

        Task UpdateAsync(UserDto entity);

        Task DeleteByIdAsync(int id);
        
        Task<ICollection<Claim>> GetUserClaimsAsync(UserDto entity);

        Task<bool?> ContainsUserWithEmailAsync(string email);
    }
}
