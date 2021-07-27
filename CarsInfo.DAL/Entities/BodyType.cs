using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table(name: "tbl.BodyType")]
	public class BodyType : BaseEntity
	{
		public string Name { get; set; }
	}
}
