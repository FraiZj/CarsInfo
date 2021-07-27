using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table(name: "tbl.Gearbox")]
	public class Gearbox : BaseEntity
	{
		public string Name { get; set; }
	}
}
