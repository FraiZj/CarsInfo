using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Domain.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public bool IsExternal { get; set; }
        
        public bool EmailVerified { get; set; }

        public UserRefreshToken UserRefreshToken { get; set; }
        
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
