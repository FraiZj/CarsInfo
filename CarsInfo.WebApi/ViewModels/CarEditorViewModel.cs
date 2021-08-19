using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels
{
    public class CarEditorViewModel
    {
        [Required]
        public string Model { get; set; }

        public string Description { get; set; }

        [Required]
        public int BrandId { get; set; }

        public ICollection<string> CarPicturesUrls { get; set; }
    }
}
