using System;

namespace CarsInfo.BLL.Models.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public UserDto User { get; set; }
    }
}
