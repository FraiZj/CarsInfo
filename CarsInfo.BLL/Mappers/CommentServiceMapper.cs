using System.Collections.Generic;
using System.Linq;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.BLL.Mappers
{
    public class CommentServiceMapper
    {
        public CommentDto MapToCommentDto(Comment comment)
        {
            if (comment is null)
            {
                return null;
            }

            return new CommentDto
            {
                Id = comment.Id,
                PublishDate = comment.PublishDate,
                Text = comment.Text
            };
        }
        
        public ICollection<CommentDto> MapToCommentsDtos(IEnumerable<Comment> comments)
        {
            return comments?.Select(MapToCommentDto).ToList();
        }

        public Comment MapToComment(CommentDto comment)
        {
            if (comment is null)
            {
                return null;
            }

            return new Comment
            {
                Id = comment.Id,
                PublishDate = comment.PublishDate,
                Text = comment.Text
            };
        }
    }
}
