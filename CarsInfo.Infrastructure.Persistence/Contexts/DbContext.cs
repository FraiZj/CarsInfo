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
        private readonly ILogger<DbContext> _logger;

        public DbContext(IDbConnection connection, ILogger<DbContext> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null) where T : BaseEntity
        {
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return null;
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T, TFirst>(string sql, Func<T, TFirst, T> map, object parameters = null) 
            where T : BaseEntity 
            where TFirst : BaseEntity
        {
            try
            {
                var res = await _connection.QueryAsync(sql, map, parameters);
                return res.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return null;
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T, TFirst, TSecond, TThird>(
            string sql, Func<T, TFirst, TSecond, TThird, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity
            where TThird : BaseEntity
        {
            try
            {
                var res = await _connection.QueryAsync(sql, map, parameters);
                return res.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return null;
            }
        }

        public async Task<IEnumerable<int>> QueryIdsAsync(string sql, object parameters = null)
        {
            try
            {
                return await _connection.QueryAsync<int>(sql, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return new List<int>();
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null) where T : BaseEntity
        {
            try
            {
                return await _connection.QueryAsync<T>(sql, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, TFirst>(string sql, Func<T, TFirst, T> map, object parameters = null) 
            where T : BaseEntity 
            where TFirst : BaseEntity
        {
            try
            {
                return await _connection.QueryAsync(sql, map, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, TFirst, TSecond>(
            string sql, 
            Func<T, TFirst, TSecond, T> map, 
            object parameters = null, 
            CommandType commandType = CommandType.Text) 
            where T : BaseEntity 
            where TFirst : BaseEntity 
            where TSecond : BaseEntity
        {
            try
            {
                return await _connection.QueryAsync(sql, map, parameters, commandType: commandType);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, TFirst, TSecond, TThird>(
            string sql, Func<T, TFirst, TSecond, TThird, T> map, object parameters = null)
            where T : BaseEntity
            where TFirst : BaseEntity
            where TSecond : BaseEntity
            where TThird : BaseEntity
        {
            try
            {
                return await _connection.QueryAsync(sql, map, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching data");
                return new List<T>();
            }
        }
        
        public async Task<int> AddAsync(string sql, object parameters = null)
        {
            try
            {
                var id = await _connection.QueryFirstOrDefaultAsync<int>($"{sql}; SELECT SCOPE_IDENTITY()", parameters);
                return id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while executing command");
                return 0;
            }
        }

        public async Task ExecuteAsync(string sql, object parameters = null)
        {
            try
            {
                await _connection.QueryFirstOrDefaultAsync($"{sql}", parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while executing command");
            }
        }

        public async Task<bool?> ContainsAsync(string sql)
        {
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<bool>(sql);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while executing command");
                return null;
            }
        }
    }
}
