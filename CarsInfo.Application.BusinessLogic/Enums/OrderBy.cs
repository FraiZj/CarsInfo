using CarsInfo.Application.Persistence.Enums;
using CarsInfo.Application.Persistence.Filters;

namespace CarsInfo.Application.BusinessLogic.Enums
{
    public static class OrderBy
    {
        public const string BrandNameAsc = "BrandNameAsc";

        public const string BrandNameDesc = "BrandNameDesc";

        public const string CarModelAsc = "CarModelAsc";
        
        public const string CarModelDesc = "CarModelDesc";

        public static SortingField ConvertToSortingField(string orderBy)
        {
            return orderBy switch
            {
                BrandNameAsc => new SortingField("Brand.Name"),
                BrandNameDesc => new SortingField("Brand.Name", Order.Descending),
                CarModelAsc => new SortingField("Car.Model"),
                CarModelDesc => new SortingField("Car.Model", Order.Descending),
                _ => null
            };
        }
    }
}
