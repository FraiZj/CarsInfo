﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table("UserRole")]
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}