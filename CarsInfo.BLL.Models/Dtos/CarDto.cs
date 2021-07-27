using System.Collections.Generic;

namespace CarsInfo.BLL.Models.Dtos
{
    public class CarDto
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string FuelType { get; set; }

        public string Country { get; set; }

        public string Gearbox { get; set; }

        public string BodyType { get; set; }

        public ICollection<string> CarPicturesUrls { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
    }
}
