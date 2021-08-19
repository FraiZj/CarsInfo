using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface ICarsRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAllWithBrandAndPicturesAsync(IList<FilterModel> filters = null, int skip = 0, int take = 6);
        
        Task<Car> GetWithAllIncludesAsync(int id);
    }
}
