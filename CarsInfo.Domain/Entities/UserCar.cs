using System.ComponentModel.DataAnnotations.Schema;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Domain.Entities
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
