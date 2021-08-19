using System.ComponentModel.DataAnnotations.Schema;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Domain.Entities
{
    [Table("Brand")]
	public class Brand : BaseEntity
	{
		public string Name { get; set; }
	}
}
