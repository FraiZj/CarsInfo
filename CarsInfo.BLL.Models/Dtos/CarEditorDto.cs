using System.Collections.Generic;

namespace CarsInfo.BLL.Models.Dtos
{
    public class CarEditorDto
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public int BrandId { get; set; }

        public int FuelTypeId { get; set; }

        public int CountryId { get; set; }

        public int GearboxId { get; set; }

        public int BodyTypeId { get; set; }

        public ICollection<string> CarPicturesUrls { get; set; }
    }
}
