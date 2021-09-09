using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IBrandService
    {
        Task<OperationResult<IEnumerable<BrandDto>>> GetAllAsync(string name);
        
        Task<OperationResult<BrandDto>> GetByIdAsync(int id);
        
        Task<OperationResult<int>> AddAsync(BrandDto entity);
        
        Task<OperationResult.OperationResult> UpdateAsync(BrandDto entity);
        
        Task<OperationResult.OperationResult> DeleteByIdAsync(int id);
    }
}
