using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table(name: "tbl.Brand")]
	public class Brand : BaseEntity
	{
		public string Name { get; set; }
	}
}
