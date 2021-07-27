using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
	[Table(name: "tbl.Country")]
	public class Country : BaseEntity
	{
		public string Name { get; set; }
	}
}
