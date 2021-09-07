using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface ICarsRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAsync(FilterModel filter = null);
        
        Task<IEnumerable<Car>> GetUserFavoriteCarsAsync(int userId, FilterModel filter = null);
        
        Task<IEnumerable<int>> GetUserFavoriteCarsIdsAsync(int userId, FilterModel filter = null);

        Task<Car> GetByIdAsync(int id, bool includeDeleted = false);
    }
}
