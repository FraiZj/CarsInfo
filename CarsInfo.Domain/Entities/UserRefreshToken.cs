using System;
using System.ComponentModel.DataAnnotations.Schema;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Domain.Entities
{
    [Table("UserRefreshToken")]
    public class UserRefreshToken : BaseEntity
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }
    }
}