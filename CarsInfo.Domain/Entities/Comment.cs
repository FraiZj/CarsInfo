using System;
using System.ComponentModel.DataAnnotations.Schema;
using CarsInfo.Domain.Entities.Base;

namespace CarsInfo.Domain.Entities
{
    [Table("Comment")]
    public class Comment : BaseEntity
    {
        public int UserId { get; set; }

        public int CarId { get; set; }

        public string Text { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public User User { get; set; }
    }
}
