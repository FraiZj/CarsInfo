using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ICarsService
    {
        Task AddToFavoriteAsync(int userId, int carId);

        Task<IEnumerable<CarDto>> GetAllAsync();

        Task<IEnumerable<CarDto>> GetAllAsync(FilterDto filter);

        Task<IEnumerable<CarDto>> GetUserCarsAsync(FilterDto filter);

        Task<CarDto> GetByIdAsync(int id);

        Task<CarEditorDto> GetCarEditorDtoByIdAsync(int id);

        Task<bool> AddAsync(CarEditorDto entity);

        Task UpdateAsync(CarEditorDto entity);

        Task DeleteByIdAsync(int id);
    }
}
