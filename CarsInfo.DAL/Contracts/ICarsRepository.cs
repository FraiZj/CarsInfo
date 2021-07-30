using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface ICarsRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAllWithBrandAndPicturesAsync();
        
        //Task<Car> GetAsyncWithAllIncludesAsync();
        
        Task<Car> GetWithAllIncludesAsync(int id);
    }
}
