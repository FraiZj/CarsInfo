using System.Collections.Generic;

namespace CarsInfo.WebApi.ViewModels.Car
{
    public class CarEditorViewModel
    {
        public string Model { get; set; }

        public string Description { get; set; }

        public int BrandId { get; set; }

        public ICollection<string> CarPicturesUrls { get; set; }
    }
}
