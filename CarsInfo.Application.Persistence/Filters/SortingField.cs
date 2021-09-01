using CarsInfo.Application.Persistence.Enums;

namespace CarsInfo.Application.Persistence.Filters
{
    public class SortingField
    {
        public SortingField(string field, Order order = Order.Ascending)
        {
            Field = field;
            Order = order;
        }

        public string Field { get; set; }
        
        public Order Order { get; set; }
    }
}
