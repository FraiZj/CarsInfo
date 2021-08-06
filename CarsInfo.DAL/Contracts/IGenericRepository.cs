using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task<T> GetAsync(int id);
        
        Task<T> GetAsync(ICollection<FilterModel> filters);
        
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(ICollection<FilterModel> filters);

        Task<IEnumerable<T>> GetAllAsync<TFirst>(ICollection<JoinModel> joins, ICollection<FilterModel> filters);

        Task<IEnumerable<T>> GetAllAsync<TFirst, TSecond>(ICollection<JoinModel> joins, ICollection<FilterModel> filters);

        Task<IEnumerable<T>> GetAllAsync<TFirst, TSecond, TThird>(ICollection<JoinModel> joins, ICollection<FilterModel> filters);

        Task<IEnumerable<T>> GetAllAsync(ICollection<Expression<Func<T, object>>> joins, ICollection<FilterModel> filters);
    }
}