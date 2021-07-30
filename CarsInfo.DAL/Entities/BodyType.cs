using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table(name: "tbl.BodyTypes")]
	public class BodyType : BaseEntity
	{
		public string Name { get; set; }
	}
}
