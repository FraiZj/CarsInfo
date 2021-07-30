using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface ICarsRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAllAsyncWithIncludes(object filter = null);

        Task<Car> GetAsyncWithIncludes(object filter);

        Task<Car> GetAsyncWithIncludes(int id);
    }
}
