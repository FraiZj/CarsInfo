using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarsInfo.Application.BusinessLogic.Enums;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class CarFilterDto
    {
        public IList<string> Brands { get; set; } = new List<string>();

        public string Model { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Skip { get; set; } = 0;

        [Range(0, 100)]
        public int Take { get; set; } = 10;

        public string OrderBy { get; set; } = CarOrderBy.BrandNameAsc;
    }
}
