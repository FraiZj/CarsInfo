using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private const string IdName = "Id";

        protected readonly IDbContext Context;
        protected readonly string TableName;

        public GenericRepository(IDbContext context)
        {
            Context = context;
            TableName = GetTableName(typeof(T));
        }

        public virtual async Task AddAsync(T entity)
        {
            var propertyContainer = ParseProperties(entity);
            var sql = $@"INSERT INTO [{TableName}] ({string.Join(", ", propertyContainer.ValueNames)}) 
                         VALUES(@{string.Join(", @", propertyContainer.ValueNames)})";
            await Context.ExecuteAsync(sql, propertyContainer.ValuePairs);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var sql = $"DELETE FROM [{TableName}] WHERE Id=@id";
            await Context.ExecuteAsync(sql, new { id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(object filter = null)
        {
            var properties = ParseProperties(filter);
            var sqlPairs = GetSqlPairs(properties.AllNames, " AND ");
            var sql = $"SELECT * FROM [{TableName}]";

            if (filter is not null)
            {
                sql += $" WHERE {sqlPairs}";
            }

            return await Context.QueryAsync<T>(sql, properties.AllPairs);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            var sql = $"SELECT * FROM [{TableName}] WHERE Id=@id";
            return await Context.QueryFirstOrDefaultAsync<T>(sql, new { id });
        }

        public virtual async Task<T> GetAsync(object filter)
        {
            var properties = ParseProperties(filter);
            var sqlPairs = GetSqlPairs(properties.AllNames, " AND ");
            var sql = $"SELECT TOP 1 * FROM [{TableName}]";

            if (filter is not null)
            {
                sql += $" WHERE {sqlPairs}";
            }

            return await Context.QueryFirstOrDefaultAsync<T>(sql, properties.AllPairs);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var propertyContainer = ParseProperties(entity);
            var sqlValuePairs = GetSqlPairs(propertyContainer.ValueNames);
            var sql = $"UPDATE [{TableName}] SET {sqlValuePairs} WHERE Id=@{entity.Id}";
            await Context.ExecuteAsync(sql, propertyContainer.AllPairs);
        }

        protected static string GetTableName(MemberInfo memberInfo)
        {
            var tableAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(TableAttribute)) as TableAttribute;
            return tableAttribute?.Name;
        }

        protected static PropertyContainer ParseProperties<TU>(TU obj)
        {
            var propertyContainer = new PropertyContainer();

            if (obj is null)
            {
                return propertyContainer;
            }

            var properties = typeof(T).GetProperties();
            
            foreach (var property in properties)
            {
                if (property.PropertyType.IsInterface)
                {
                    continue;
                }

                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    continue;
                }

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name)?.GetValue(obj, null);

                if (name == IdName)
                {
                    propertyContainer.AddId(name, value);
                }
                else
                {
                    propertyContainer.AddValue(name, value);
                }
            }

            return propertyContainer;
        }

        protected static string GetSqlPairs(IEnumerable<string> keys, string separator = ", ")
        {
            var pairs = keys.Select(key => $"{key}=@{key}").ToList();
            return string.Join(separator, pairs);
        }

        protected class PropertyContainer
        {
            private readonly Dictionary<string, object> _ids;
            private readonly Dictionary<string, object> _values;

            internal PropertyContainer()
            {
                _ids = new Dictionary<string, object>();
                _values = new Dictionary<string, object>();
            }

            internal IEnumerable<string> IdNames => _ids.Keys;

            internal IEnumerable<string> ValueNames => _values.Keys;

            internal IEnumerable<string> AllNames => _ids.Keys.Union(_values.Keys);

            internal IDictionary<string, object> IdPairs => _ids;

            internal IDictionary<string, object> ValuePairs => _values;

            internal IEnumerable<KeyValuePair<string, object>> AllPairs => _ids.Concat(_values);

            internal void AddId(string name, object value)
            {
                _ids.Add(name, value);
            }

            internal void AddValue(string name, object value)
            {
                _values.Add(name, value);
            }
        }
    }
}
