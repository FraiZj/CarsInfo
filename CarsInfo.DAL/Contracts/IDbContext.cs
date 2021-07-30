using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Contracts
{
    public interface IDbContext
    {
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null) where T : BaseEntity;

        Task<TReturn> QueryFirstOrDefaultAsync<T, TFirst, TSecond, TThird, TReturn>(
            string sql, Func<T, TFirst, TSecond, TThird, TReturn> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity
            where TThird : BaseEntity
            where TReturn : BaseEntity;
        
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null) where T : BaseEntity;

        Task<IEnumerable<TReturn>> QueryAsync<T, TFirst, TSecond, TThird, TReturn>(
            string sql, Func<T, TFirst, TSecond, TThird, TReturn> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity
            where TThird : BaseEntity
            where TReturn : BaseEntity;

        Task<int> ExecuteAsync(string sql, object parameters = null);
    }
}
