using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using Dapper;

namespace CarsInfo.DAL
{
	public class DapperContext : IContext
	{
        private string _connectionString = "";

        public async Task AddAsync<T>(T entity) where T : BaseEntity
        {
            var tableName = GetTableName(typeof(T));
            var sql = $"INSERT INTO {tableName} ... VALUES ...;";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync<T>(int id) where T : BaseEntity
        {
            var tableName = GetTableName(typeof(T));
            var sql = $"DELETE FROM {tableName} WHERE Id = @id;";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { id });
        }

        public Task<IEnumerable<T>> GetAllAsync<T>(IEnumerable<Type> includes) where T : BaseEntity
        {
            var tableName = GetTableName(typeof(T));
            using var connection = new SqlConnection(_connectionString);
            var sql = new StringBuilder($"SELECT * FROM {tableName}");

            foreach (var include in includes)
            {
                var includeTableName = GetTableName(include);
                var inludeSql = $" INNER JOIN {includeTableName} ON {tableName}.Id = {includeTableName}.Id";
                sql.Append(inludeSql);
            }

            return Task.FromResult<IEnumerable<T>>(Array.Empty<T>());
        }

        public Task<T> GetAsync<T>() where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            var tableName = GetTableName(typeof(T));
            var sql = $"UPDATE {tableName} SET ... WHERE Id = @id;";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, new { id = entity.Id });
        }

        private string GetTableName(Type type)
        {
			var tableAttribute = Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute;
			return tableAttribute?.Name;
		}
	}
}
