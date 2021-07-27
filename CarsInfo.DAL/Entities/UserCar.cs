using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table(name: "tbl.UserCar")]
    public class UserCar
    {
        public int UserId { get; set; }

        public int CarId { get; set; }

        public User User { get; set; }
        
        public Car Car { get; set; }
    }
}
