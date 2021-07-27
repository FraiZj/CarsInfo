using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Models.Dtos;

namespace CarsInfo.BLL.Contracts
{
    public interface IBrandService
    {
        public Task<IEnumerable<BrandDto>> GetAllAsync();
        public Task<BrandDto> GetByIdAsync(int id);
        public Task AddAsync(BrandDto entity);
        public Task UpdateAsync(BrandDto entity);
        public Task DeleteByIdAsync(int id);
    }
}
