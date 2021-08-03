using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table("FuelType")]
	public class FuelType : BaseEntity
	{
		public string Name { get; set; }
	}
}
