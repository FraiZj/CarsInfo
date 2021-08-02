using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public ICollection<Car> Cars { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
