using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<int> AddAsync(T entity);
        
        Task AddRangeAsync(IList<T> entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task DeleteRangeAsync(IEnumerable<int> ids);

        Task<T> GetAsync(int id, bool includeDeleted = false);

        Task<T> GetAsync(IList<FiltrationField> filters, bool includeDeleted = false);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(FilterModel filterModel);

        Task<bool?> ContainsAsync(IList<FiltrationField> filters);
    }
}
