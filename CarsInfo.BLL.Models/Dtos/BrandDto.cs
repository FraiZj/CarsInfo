using System.ComponentModel.DataAnnotations;

namespace CarsInfo.BLL.Models.Dtos
{
    public class BrandDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
