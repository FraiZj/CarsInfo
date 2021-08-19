using System;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public UserDto User { get; set; }
    }
}
