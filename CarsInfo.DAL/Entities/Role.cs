using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
