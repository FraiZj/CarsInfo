using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.Persistence.Filters;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class FilterService : IFilterService
    {
        public FilterModel ConfigureCarFilter(FilterDto filter)
        {
            var filterModel = new FilterModel()
            {
                Skip = filter.Skip,
                Take = filter.Take
            };

            if (filter.Brands.Any())
            {
                var brands = string.Join(", ", filter.Brands.Select(b => $"'{b.ToLower()}'"));
                filterModel.Filters.Add(new FiltrationField("LOWER(Brand.Name)", $"({brands})", "IN", "AND"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Model))
            {
                filterModel.Filters.Add(new FiltrationField("LOWER(Car.Model)", $"%{filter.Model.ToLower()}%", "LIKE"));
            }



            return filterModel;
        }
    }
}
