using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
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

        //public virtual async Task<IEnumerable<T>> GetAllAsync(object filter = null)
        //{
        //    var properties = ParseProperties(filter);
        //    var sqlPairs = GetSqlPairs(properties.AllNames, " AND ");
        //    var sql = $"SELECT * FROM [{TableName}]";

        //    if (filter is not null)
        //    {
        //        sql += $" WHERE {sqlPairs}";
        //    }

        //    return await Context.QueryAsync<T>(sql, properties.AllPairs);
        //}

        public async Task<T> GetAsync(ICollection<FilterModel> filters)
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

        public async Task<IEnumerable<T>> GetAllAsync(ICollection<JoinModel> joins, ICollection<FilterModel> filters)
        {
            var filter = ConfigureFilter(filters);
            var join = ConfigureJoins(joins);
            var sql = $"SELECT * FROM [{TableName}] {join} {filter}";
            return await Context.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<T>> GetAllAsync<TFirst, TSecond>(ICollection<Expression<Func<T, object>>> joins, ICollection<FilterModel> filters)
        {
            var join = GetJoins(joins);
            var sql = $"SELECT * FROM [{TableName}] {join}";
            return await Context.QueryAsync<T>(sql);
        }

        private string GetJoins(IEnumerable<Expression<Func<T, object>>> joins)
        {
            var result = new StringBuilder();

            foreach (var join in joins)
            {
                var joinTable = join.GetReturnType().Name;
                result.Append(@$"LEFT JOIN {joinTable} ON {joinTable}.Id = {TableName}.Id ");
            }

            return result.ToString();
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

        protected string ConfigureFilter(IEnumerable<FilterModel> filters)
        {
            var result = new StringBuilder("WHERE ");

            foreach (var filter in filters)
            {
                filter.Value = filter.Value is string ? 
                    new string($"'{filter.Value}'") : 
                    filter.Value;

                result.Append($"{filter.Field} {filter.Operator} {filter.Value} ");

                if (!string.IsNullOrEmpty(filter.Separator))
                {
                    result.Append($"{filter.Separator} ");
                }
            }

            return result.ToString();
        }

        protected string ConfigureJoins(IEnumerable<JoinModel> joins)
        {
            var result = new StringBuilder();

            foreach (var join in joins)
            {
                result.Append(@$"LEFT JOIN {join.ForeignTable} 
                    ON {join.ForeignTable}.{join.ForeignField} = {TableName}.{join.PrimaryField} ");
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
