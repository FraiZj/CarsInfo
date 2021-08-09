using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table("UserCar")]
    public class UserCar : BaseEntity
    {
        public int UserId { get; set; }

        public int CarId { get; set; }

        public User User { get; set; }

        public Car Car { get; set; }
    }
}
