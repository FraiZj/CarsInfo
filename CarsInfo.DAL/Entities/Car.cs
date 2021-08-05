using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table("Car")]
	public class Car : BaseEntity
	{
		public int BrandId { get; set; }

		public string Model { get; set; }
		
		public string Description { get; set; }

		public Brand Brand { get; set; }

		public ICollection<User> Users { get; set; }

        public ICollection<CarPicture> CarPictures { get; set; } = new List<CarPicture>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
