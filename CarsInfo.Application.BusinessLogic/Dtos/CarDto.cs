using System.Collections.Generic;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class CarDto
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }
        
        public string Brand { get; set; }

        public ICollection<string> CarPicturesUrls { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
    }
}
