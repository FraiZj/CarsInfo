using System.ComponentModel.DataAnnotations.Schema;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Domain.Entities
{
    [Table("CarPicture")]
    public class CarPicture : BaseEntity
	{
		public int CarId { get; set; }

		public string PictureLink { get; set; }

        public Car Car { get; set; }
    }
}
