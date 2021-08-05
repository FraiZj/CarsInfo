using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
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
        
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null) where T : BaseEntity;

        Task<IEnumerable<T>> QueryAsync<T, TFirst>(
            string sql, Func<T, TFirst, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity;

        Task<IEnumerable<T>> QueryAsync<T, TFirst, TSecond>(
            string sql, Func<T, TFirst, TSecond, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity;

        Task<int> ExecuteAsync(string sql, object parameters = null);
    }
}
