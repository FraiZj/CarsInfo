using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync(string name);
        Task<BrandDto> GetByIdAsync(int id);
        Task AddAsync(BrandDto entity);
        Task UpdateAsync(BrandDto entity);
        Task DeleteByIdAsync(int id);
    }
}
