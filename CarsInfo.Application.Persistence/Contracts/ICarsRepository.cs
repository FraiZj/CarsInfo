using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface ICarsRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetUserFavoriteCarsAsync(int userId, FilterModel filter = null);
    }
}
