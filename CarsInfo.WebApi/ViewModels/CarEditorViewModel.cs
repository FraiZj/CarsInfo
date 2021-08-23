using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels
{
    public class CarEditorViewModel
    {
        [Required, MaxLength(50)]
        public string Model { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required, MinLength(1)]
        public ICollection<string> CarPicturesUrls { get; set; }
    }
}
