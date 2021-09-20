using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.Application.Persistence.Filters;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class FilterService : IFilterService
    {
        public FilterModel ConfigureCarFilter(CarFilterDto carFilter)
        {
            var filterModel = new FilterModel
            {
                Skip = carFilter.Skip,
                Take = carFilter.Take
            };

            if (carFilter.Brands.Any())
            {
                var brands = string.Join(", ", carFilter.Brands.Select(b => $"'{b.ToLower()}'"));
                filterModel.Filters.Add(new FiltrationField("LOWER(Brand.Name)", $"({brands})", "IN", "AND"));
            }

            if (!string.IsNullOrWhiteSpace(carFilter.Model))
            {
                filterModel.Filters.Add(new FiltrationField("LOWER(Car.Model)", $"%{carFilter.Model.ToLower()}%", "LIKE"));
            }

            if (!string.IsNullOrWhiteSpace(carFilter.OrderBy))
            {
                filterModel.OrderBy = CarOrderBy.ConvertToSortingField(carFilter.OrderBy);
            }

            return filterModel;
        }

        public FilterModel ConfigureCommentFilter(int carId, CommentFilterDto filter)
        {
            var filterModel = new FilterModel
            {
                Skip = filter.Skip,
                Take = filter.Take
            };

            if (!string.IsNullOrWhiteSpace(filter.OrderBy))
            {
                filterModel.OrderBy = CommentOrderBy.ConvertToSortingField(filter.OrderBy);
            }

            filterModel.Filters.Add(new FiltrationField("Comment.CarId", carId));

            return filterModel;
        }
    }
}
