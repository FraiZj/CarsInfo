using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Domain.Entities
{
	[Table("Car")]
	public class Car : BaseEntity
	{
		public int BrandId { get; set; }

		public string Model { get; set; }
		
		public string Description { get; set; }

		public Brand Brand { get; set; }

        public ICollection<CarPicture> CarPictures { get; set; } = new List<CarPicture>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
