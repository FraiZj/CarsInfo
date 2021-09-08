using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.ViewModels;
using CarsInfo.WebApi.ViewModels.Comment;

namespace CarsInfo.WebApi.Mappers
{
    public class CommentControllerMapper
    {
        public CommentEditorDto MapToCommentEditorDto(CommentEditorViewModel comment)
        {
            return new CommentEditorDto
            {
                Text = comment.Text
            };
        }
        
        public CommentViewModel MapToCommentViewModel(CommentDto comment)
        {
            return new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                PublishDate = comment.PublishDate,
                UserName = $"{comment.User.FirstName} {comment.User.LastName}"
            };
        }

        public IEnumerable<CommentViewModel> MapToCommentViewModels(IEnumerable<CommentDto> comments)
        {
            return comments.Select(MapToCommentViewModel);
        }
    }
}