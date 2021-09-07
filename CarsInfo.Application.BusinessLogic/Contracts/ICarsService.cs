using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ICarsService
    {
        Task<ToggleFavoriteStatus> ToggleFavoriteAsync(int userId, int carId);

        Task<IEnumerable<CarDto>> GetAllAsync();

        Task<IEnumerable<CarDto>> GetAllAsync(FilterDto filter);
        
        Task<IEnumerable<CarDto>> GetUserFavoriteCarsAsync(int userId, FilterDto filter);
        
        Task<IEnumerable<int>> GetUserFavoriteCarsIdsAsync(int userId);
        
        Task<CarDto> GetByIdAsync(int id);

        Task<CarEditorDto> GetCarEditorDtoByIdAsync(int id);

        Task<bool> AddAsync(CarEditorDto entity);

        Task UpdateAsync(CarEditorDto entity);

        Task DeleteByIdAsync(int id);
    }
}
