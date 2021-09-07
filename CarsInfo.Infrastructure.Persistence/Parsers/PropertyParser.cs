using System.Collections.Generic;
using System.Linq;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Infrastructure.Persistence.Parsers
{
    public static class PropertyParser<T> where T : BaseEntity
    {
        public static PropertyContainer ParseProperties<TU>(TU obj)
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

                if (name == "Id")
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
        
        public class PropertyContainer
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