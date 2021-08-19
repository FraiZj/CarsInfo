using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<int> AddAsync(T entity);
        
        Task AddRangeAsync(IList<T> entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task DeleteRangeAsync(IEnumerable<int> ids);

        Task<T> GetAsync(int id);

        Task<T> GetAsync(IList<FilterModel> filters);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(IList<FilterModel> filters);

        Task<bool?> ContainsAsync(IList<FilterModel> filters);
    }
}
