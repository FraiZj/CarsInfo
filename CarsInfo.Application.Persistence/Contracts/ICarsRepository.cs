using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface ICarsRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAllWithBrandAndPicturesAsync(FilterModel filter = null);
        
        Task<IEnumerable<Car>> GetUserCarsAsync(string userId, FilterModel filter = null);

        Task<Car> GetWithAllIncludesAsync(int id, bool includeDeleted = false);
    }
}
