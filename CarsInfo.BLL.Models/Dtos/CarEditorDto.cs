using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsInfo.BLL.Models.Dtos
{
    public class CarEditorDto
    {
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        public string Description { get; set; }

        [Required]
        public int BrandId { get; set; }

        public ICollection<string> CarPicturesUrls { get; set; }
    }
}
