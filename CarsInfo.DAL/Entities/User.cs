using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table(name: "tbl.Users")]
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public ICollection<UserCar> UserCars { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
