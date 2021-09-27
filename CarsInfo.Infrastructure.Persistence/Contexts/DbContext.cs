using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Domain.Entities.Base;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.Persistence.Contexts
{
    public class DbContext : IDbContext
    {
        private readonly IDbConnection _connection;

        public DbContext(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null) where T : BaseEntity
        {
            return _connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T, TFirst>(string sql, Func<T, TFirst, T> map, object parameters = null) 
            where T : BaseEntity 
            where TFirst : BaseEntity
        {
            var res = await _connection.QueryAsync(sql, map, parameters);
            return res.FirstOrDefault();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T, TFirst, TSecond, TThird>(
            string sql, Func<T, TFirst, TSecond, TThird, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity
            where TThird : BaseEntity
        {
            var res = await _connection.QueryAsync(sql, map, parameters);
            return res.FirstOrDefault();
        }

        public Task<IEnumerable<int>> QueryIdsAsync(string sql, object parameters = null)
        {
            return _connection.QueryAsync<int>(sql, parameters);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null) where T : BaseEntity
        {
            return _connection.QueryAsync<T>(sql, parameters);
        }

        public Task<IEnumerable<T>> QueryAsync<T, TFirst>(string sql, Func<T, TFirst, T> map, object parameters = null) 
            where T : BaseEntity 
            where TFirst : BaseEntity
        {
            return _connection.QueryAsync(sql, map, parameters);
        }

        public Task<IEnumerable<T>> QueryAsync<T, TFirst, TSecond>(
            string sql, 
            Func<T, TFirst, TSecond, T> map, 
            object parameters = null, 
            CommandType commandType = CommandType.Text) 
            where T : BaseEntity 
            where TFirst : BaseEntity 
            where TSecond : BaseEntity
        {
            return _connection.QueryAsync(sql, map, parameters, commandType: commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T, TFirst, TSecond, TThird>(
            string sql, Func<T, TFirst, TSecond, TThird, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity
            where TThird : BaseEntity
        {
            return _connection.QueryAsync(sql, map, parameters);
        }
        
        public async Task<int> AddAsync(string sql, object parameters = null)
        {
            var id = await _connection.QueryFirstOrDefaultAsync<int>($"{sql}; SELECT SCOPE_IDENTITY()", parameters);
            return id;
        }

        public Task ExecuteAsync(string sql, object parameters = null)
        {
            return _connection.QueryFirstOrDefaultAsync($"{sql}", parameters);
        }

        public Task<bool> ContainsAsync(string sql)
        {
            return _connection.QueryFirstOrDefaultAsync<bool>(sql);
        }
    }
}
