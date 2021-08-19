using System.ComponentModel.DataAnnotations;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class BrandDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
