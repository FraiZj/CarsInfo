using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table("Gearbox")]
	public class Gearbox : BaseEntity
	{
		public string Name { get; set; }
	}
}
