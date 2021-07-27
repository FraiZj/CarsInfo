using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table(name: "tbl.Cars")]
	public class Car : BaseEntity
	{
		public int BrandId { get; set; }

		public string Model { get; set; }
		
		public string Description { get; set; }

		public int FuelTypeId { get; set; }

		public int CountryId { get; set; }

		public int GearboxId { get; set; }

		public int BodyTypeId { get; set; }

		public Brand Brand { get; set; }
		
		public FuelType FuelType { get; set; }
		
		public Country Country { get; set; }

		public Gearbox Gearbox { get; set; }

		public BodyType BodyType { get; set; }

		public ICollection<UserCar> UserCars { get; set; }
		
		public ICollection<CarPicture> CarPictures { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
