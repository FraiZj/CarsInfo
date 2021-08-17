using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CarsInfo.DAL.Assistance;
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

        public virtual async Task<int> AddAsync(T entity)
        {
            var propertyContainer = ParseProperties(entity);
            var sql = $@"INSERT INTO [{TableName}] ({string.Join(", ", propertyContainer.ValueNames)}) 
                         VALUES(@{string.Join(", @", propertyContainer.ValueNames)})";
            return await Context.ExecuteAsync(sql, propertyContainer.ValuePairs);
        }

        public async Task AddRangeAsync(IList<T> entities)
        {
            var propertyContainer = ParseProperties(entities.First());
            var sql = $@"INSERT INTO [{TableName}] ({string.Join(", ", propertyContainer.ValueNames)}) 
                         VALUES {CombineValuesToInsert(entities, propertyContainer)}";
            await Context.ExecuteAsync(sql);
        }

        protected string CombineValuesToInsert(IList<T> entities, PropertyContainer propertyContainer)
        {
            var result = new List<string>();

            foreach (var entity in entities)
            {
                var rowValues = new List<object>();
                
                foreach (var valueName in propertyContainer.ValueNames)
                {
                    var value = typeof(T).GetProperty(valueName)?.GetValue(entity);
                    value = value is string ? new string($"'{value}'") : value;
                    value = value is bool ? Convert.ToByte(value) : value;
                    rowValues.Add(value);
                }

                result.Add($"({string.Join(", ", rowValues)})");
            }

            return string.Join(", ", result);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var sql = $"DELETE FROM [{TableName}] WHERE Id=@id";
            await Context.ExecuteAsync(sql, new { id });
        }

        public async Task<T> GetAsync(IList<FilterModel> filters)
        {
            var filter = ConfigureFilter(filters);
            var sql = $"SELECT TOP 1 * FROM [{TableName}] {filter}";
            return await Context.QueryFirstOrDefaultAsync<T>(sql);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {TableName}";

            return await Context.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<T>> GetAllAsync(IList<FilterModel> filters)
        {
            var sql = $"SELECT * FROM [{TableName}]";

            if (filters is not null && filters.Any())
            {
                var filter = ConfigureFilter(filters);
                sql += $" {filter}";
            }
            
            return await Context.QueryAsync<T>(sql);
        }

        public async Task<bool?> ContainsAsync(IList<FilterModel> filters)
        {
            if (filters is not null && filters.Any())
            {
                return null;
            }

            var filter = ConfigureFilter(filters);

            var sql = @$"
                        SELECT
                          CASE WHEN EXISTS 
                          (
                                SELECT * FROM {GetTableName(typeof(T))} {filter}
                          )
                          THEN 1
                          ELSE 0
                        END";

            return await Context.ContainsAsync(sql);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            var sql = $"SELECT * FROM [{TableName}] WHERE Id=@id";
            return await Context.QueryFirstOrDefaultAsync<T>(sql, new { id });
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

        protected string ConfigureFilter(IList<FilterModel> filters)
        {
            var result = new StringBuilder("WHERE ");

            for (var i = 0; i < filters.Count; i++)
            {
                var filter = filters[i];
                filter.Value = filter.Value is string && filter.Operator != "IN" ?
                    new string($"'{filter.Value}'") :
                    filter.Value;

                result.Append($"{filter.Field} {filter.Operator} {filter.Value} ");

                if (!string.IsNullOrEmpty(filter.Separator) && i != filters.Count - 1)
                {
                    result.Append($"{filter.Separator} ");
                }
            }

            return result.ToString();
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

        protected static string GetSqlPairs(IEnumerable<string> keys, string tableAlias = null, string separator = ", ")
        {
            var prefix = tableAlias is null ? string.Empty : $"{tableAlias}.";
            var pairs = keys.Select(key => $"{prefix}{key}=@{key}").ToList();
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
