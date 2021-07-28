using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Models.Dtos;

namespace CarsInfo.BLL.Contracts
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<BrandDto> GetByIdAsync(int id);
        Task AddAsync(BrandDto entity);
        Task UpdateAsync(BrandDto entity);
        Task DeleteByIdAsync(int id);
    }
}
