namespace CarsInfo.DAL.Entities
{
	public class CarProfile
	{
		public int CarId { get; set; }
		public string Description { get; set; }
		public int FuelTypeId { get; set; }
		public int CountryId { get; set; }
		public int GearboxId { get; set; }
		public int BodyTypeId { get; set; }
	}
}
