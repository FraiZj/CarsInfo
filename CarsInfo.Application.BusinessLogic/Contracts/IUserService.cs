using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IUserService
    {
        Task<OperationResult<IEnumerable<UserDto>>> GetAllAsync();

        Task<OperationResult<UserDto>> GetByEmailAsync(string email);
        
        Task<OperationResult<int>> AddAsync(UserDto entity);

        Task<OperationResult.OperationResult> UpdateAsync(UserDto entity);

        Task<OperationResult.OperationResult> DeleteByIdAsync(int id);
        
        Task<OperationResult<ICollection<Claim>>> GetUserClaimsAsync(UserDto entity);

        Task<OperationResult<bool>> ContainsUserWithEmailAsync(string email);
    }
}
