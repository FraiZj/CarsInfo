using System.Collections.Generic;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.BusinessLogic.Mappers.Contracts
{
    public interface ICommentServiceMapper
    {
        CommentDto MapToCommentDto(Comment comment);

        ICollection<CommentDto> MapToCommentsDtos(IEnumerable<Comment> comments);

        Comment MapToComment(CommentDto comment);

        Comment MapToComment(CommentEditorDto commentEditorDto);
    }
}