using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities.Base;
using CarsInfo.Infrastructure.Persistence.Configurators;
using CarsInfo.Infrastructure.Persistence.Parsers;

namespace CarsInfo.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IDbContext Context;
        protected readonly string TableName;

        public GenericRepository(IDbContext context)
        {
            Context = context;
            TableName = SqlQueryConfigurator.GetTableName<T>();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            var propertyContainer = PropertyParser<T>.ParseProperties(entity);
            var sql = $@"INSERT INTO [{TableName}] ({string.Join(", ", propertyContainer.ValueNames)}) 
                         VALUES(@{string.Join(", @", propertyContainer.ValueNames)})";
            return await Context.AddAsync(sql, propertyContainer.ValuePairs);
        }

        public virtual async Task AddRangeAsync(IList<T> entities)
        {
            var propertyContainer = PropertyParser<T>.ParseProperties(entities.First());
            var sql = $@"INSERT INTO [{TableName}] ({string.Join(", ", propertyContainer.ValueNames)}) 
                         VALUES {SqlQueryConfigurator.CombineValuesToInsert(entities, propertyContainer)}";
            await Context.ExecuteAsync(sql);
        }
        
        public virtual async Task DeleteAsync(int id)
        {
            var sql = $"UPDATE [{TableName}] SET IsDeleted = 1 WHERE Id=@id";
            await Context.ExecuteAsync(sql, new { id });
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<int> ids)
        {
            var filters = new List<FiltrationField>
            {
                new("Id", $"({string.Join(", ", ids)})", "IN")
            };
            var filter = SqlQueryConfigurator.ConfigureFilter(TableName, filters);
            var sql = $"UPDATE [{TableName}] SET IsDeleted = 1 {filter}";
            await Context.ExecuteAsync(sql);
        }

        public virtual async Task<T> GetAsync(IList<FiltrationField> filters, bool includeDeleted = false)
        {
            if (!filters?.Any() ?? true)
            {
                return null;
            }
            
            var filter = SqlQueryConfigurator.ConfigureFilter(TableName, filters, includeDeleted);
            var sql = $"SELECT TOP 1 * FROM [{TableName}] {filter}";
            return await Context.QueryFirstOrDefaultAsync<T>(sql);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {TableName}";

            return await Context.QueryAsync<T>(sql);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(FilterModel filterModel)
        {
            var filters = SqlQueryConfigurator.ConfigureFilter(
                TableName, filterModel.Filters, filterModel.IncludeDeleted);
            var orderBy = SqlQueryConfigurator.ConfigureOrderBy(filterModel.OrderBy);
            var sql = $@"SELECT * FROM [{TableName}]
                        { filters }
	                    { orderBy }";
            
            return await Context.QueryAsync<T>(sql);
        }

        public virtual async Task<bool> ContainsAsync(IList<FiltrationField> filters)
        {
            if (filters is not null && !filters.Any())
            {
                return false;
            }

            var filter = SqlQueryConfigurator.ConfigureFilter(TableName, filters);

            var sql = @$"
                        SELECT
                          CASE WHEN EXISTS 
                          (
                            SELECT * FROM [{TableName}] {filter}
                          )
                          THEN 1
                          ELSE 0
                        END";
 
            return await Context.ContainsAsync(sql);
        }

        public virtual async Task<T> GetAsync(int id, bool includeDeleted = false)
        {
            var sql = $"SELECT * FROM [{TableName}] WHERE Id=@id";

            if (!includeDeleted)
            {
                sql += " AND IsDeleted = 0";
            }
            
            return await Context.QueryFirstOrDefaultAsync<T>(sql, new { id });
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var propertyContainer = PropertyParser<T>.ParseProperties(entity);
            var sqlValuePairs = SqlQueryConfigurator.GetSqlPairs(propertyContainer.ValueNames);
            var sql = $"UPDATE [{TableName}] SET {sqlValuePairs} WHERE Id={entity.Id}";
            await Context.ExecuteAsync(sql, propertyContainer.AllPairs.ToList());
        }
    }
}
