using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CarsInfo.Application.Persistence.Enums;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities.Base;
using CarsInfo.Infrastructure.Persistence.Parsers;

namespace CarsInfo.Infrastructure.Persistence.Configurators
{
    public static class SqlQueryConfigurator
    {
        public static string GetTableName<T>()
            where T : BaseEntity
        {
            var tableAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute)) as TableAttribute;
            return tableAttribute?.Name;
        }
        
        public static string ConfigureOrderBy(SortingField sortingField)
        {
            if (sortingField is null || string.IsNullOrWhiteSpace(sortingField.Field))
            {
                return string.Empty;
            }

            var order = sortingField.Order == Order.Ascending ? "ASC" : "DESC";
            return $"ORDER BY {sortingField.Field} {order}";
        }

        public static string ConfigureFilter(
            string tableName, 
            IList<FiltrationField> filters, 
            bool includeDeleted = false)
        {
            if (includeDeleted && !filters.Any())
            {
                return string.Empty;
            }

            var result = new StringBuilder("WHERE ");

            if (!includeDeleted)
            {
                result.Append($"[{tableName}].IsDeleted = 0 ");

                if (filters.Any())
                {
                    result.Append("AND ");
                }
            }

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
        
        public static string GetSqlPairs(IEnumerable<string> keys, string tableAlias = null, string separator = ", ")
        {
            var prefix = tableAlias is null ? string.Empty : $"{tableAlias}.";
            var pairs = keys.Select(key => $"{prefix}{key}=@{key}").ToList();
            return string.Join(separator, pairs);
        }
        
        public static string CombineValuesToInsert<T>(
            IList<T> entities, 
            PropertyParser<T>.PropertyContainer propertyContainer)
            where T : BaseEntity
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
    }
}