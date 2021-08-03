using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Models.Dtos;

namespace CarsInfo.BLL.Contracts
{
    public interface ICarsService
    {
        Task<IEnumerable<CarDto>> GetAllAsync();

        Task<CarDto> GetByIdAsync(int id);

        Task<CarEditorDto> GetCarEditorDtoByIdAsync(int id);

        Task AddAsync(CarEditorDto entity);

        Task UpdateAsync(CarEditorDto entity);

        Task DeleteByIdAsync(int id);
    }
}
