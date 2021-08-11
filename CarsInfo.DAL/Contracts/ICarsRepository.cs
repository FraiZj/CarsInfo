using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface ICarsRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAllWithBrandAndPicturesAsync(IList<FilterModel> filters = null);
        
        Task<Car> GetWithAllIncludesAsync(int id);
    }
}
