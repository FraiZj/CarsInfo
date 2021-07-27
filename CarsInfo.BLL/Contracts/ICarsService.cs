using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Models.Dtos;

namespace CarsInfo.BLL.Contracts
{
    public interface ICarsService
    {
        public Task<IEnumerable<CarDto>> GetAllAsync();
        public Task<CarDto> GetByIdAsync(int id);
        public Task AddAsync(CarEditorDto entity);
        public Task UpdateAsync(CarEditorDto entity);
        public Task DeleteByIdAsync(int id);
    }
}
