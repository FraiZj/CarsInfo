using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.Application.BusinessLogic.OperationResult;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface ICarsService
    {
        Task<OperationResult<ToggleFavoriteStatus>> ToggleFavoriteAsync(int userId, int carId);
        
        Task<OperationResult<IEnumerable<CarDto>>> GetAllAsync();

        Task<OperationResult<IEnumerable<CarDto>>> GetAllAsync(CarFilterDto carFilter);
        
        Task<OperationResult<IEnumerable<CarDto>>> GetUserFavoriteCarsAsync(int userId, CarFilterDto carFilter);
        
        Task<OperationResult<IEnumerable<int>>> GetUserFavoriteCarsIdsAsync(int userId);
        
        Task<OperationResult<CarDto>> GetByIdAsync(int id);

        Task<OperationResult<CarEditorDto>> GetCarEditorDtoByIdAsync(int id);

        Task<OperationResult<int>> AddAsync(CarEditorDto entity);

        Task<OperationResult.OperationResult> UpdateAsync(CarEditorDto entity);

        Task<OperationResult.OperationResult> DeleteByIdAsync(int id);
    }
}
