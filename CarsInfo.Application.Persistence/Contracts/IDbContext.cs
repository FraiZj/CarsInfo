using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Application.Persistence.Contracts
{
    public interface IDbContext
    {
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null) where T : BaseEntity;

        Task<T> QueryFirstOrDefaultAsync<T, TFirst>(
            string sql, Func<T, TFirst, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity;

        Task<T> QueryFirstOrDefaultAsync<T, TFirst, TSecond, TThird>(
            string sql, Func<T, TFirst, TSecond, TThird, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity
            where TThird : BaseEntity;
        
        Task<IEnumerable<int>> QueryIdsAsync(string sql, object parameters = null);
        
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null) where T : BaseEntity;

        Task<IEnumerable<T>> QueryAsync<T, TFirst>(
            string sql, Func<T, TFirst, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity;

        Task<IEnumerable<T>> QueryAsync<T, TFirst, TSecond>(
            string sql, 
            Func<T, TFirst, TSecond, T> map, 
            object parameters = null,
            CommandType commandType = CommandType.Text)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity;

        Task<int> AddAsync(string sql, object parameters = null);
        
        Task ExecuteAsync(string sql, object parameters = null);

        Task<bool> ContainsAsync(string sql);
    }
}
