using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<int> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task<T> GetAsync(int id);

        Task<T> GetAsync(ICollection<FilterModel> filters);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(ICollection<FilterModel> filters);
    }
}
