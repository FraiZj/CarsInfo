using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table(name: "tbl.CarPictures")]
    public class CarPicture : BaseEntity
	{
		public int CarId { get; set; }

		public string PictureLink { get; set; }

        public Car Car { get; set; }
    }
}
