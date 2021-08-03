using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table("Brand")]
	public class Brand : BaseEntity
	{
		public string Name { get; set; }
	}
}
