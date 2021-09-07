using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers.Contracts;

namespace CarsInfo.Infrastructure.BusinessLogic.Mappers
{
    public class CommentServiceMapper
    {
        private readonly UserServiceMapper _userServiceMapper;

        public CommentServiceMapper(UserServiceMapper userServiceMapper)
        {
            _userServiceMapper = userServiceMapper;
        }
        
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
                Text = comment.Text,
                UserId = comment.UserId,
                CarId = comment.CarId,
                User = _userServiceMapper.MapToUserDto(comment.User)
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

        public Comment MapToComment(CommentEditorDto commentEditorDto)
        {
            if (commentEditorDto is null)
            {
                return null;
            }

            return new Comment
            {
                Text = commentEditorDto.Text,
                UserId = commentEditorDto.UserId,
                CarId = commentEditorDto.CarId
            };
        }
    }
}
