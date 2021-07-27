using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsInfo.DAL.Entities
{
    [Table(name: "tbl.Comments")]
    public class Comment : BaseEntity
    {
        public int UserId { get; set; }

        public string Text { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public User User { get; set; }
    }
}
