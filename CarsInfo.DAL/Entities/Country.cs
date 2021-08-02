using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table("Country")]
	public class Country : BaseEntity
	{
		public string Name { get; set; }
	}
}
