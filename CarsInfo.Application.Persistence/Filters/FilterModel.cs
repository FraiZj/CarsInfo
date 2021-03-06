using System.Collections.Generic;

namespace CarsInfo.Application.Persistence.Filters
{
    public class FilterModel
    {
        public FilterModel() { }
        
        public FilterModel(params FiltrationField[] filters)
        {
            Filters = filters;
        }
        
        public bool IncludeDeleted { get; set; } = false;

        public IList<FiltrationField> Filters { get; set; } = new List<FiltrationField>();

        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 6;

        public SortingField OrderBy = null;
    }
}
