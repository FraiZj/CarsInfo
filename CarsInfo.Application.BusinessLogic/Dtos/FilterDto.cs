using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class FilterDto
    {
        public IList<string> Brands { get; set; } = new List<string>();

        public string Model { get; set; } = string.Empty;

        [MinLength(0)]
        public int Skip { get; set; } = 0;

        [MinLength(0), MaxLength(100)]
        public int Take { get; set; } = 3;

        public string OrderBy { get; set; } = Enums.OrderBy.BrandNameAsc;
    }
}
