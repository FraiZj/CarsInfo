using CarsInfo.WebApi.ViewModels.Comment;
using FluentValidation;

namespace CarsInfo.WebApi.Validators.Comment
{
    public class CommentEditorViewModelValidator : AbstractValidator<CommentEditorViewModel>
    {
        public CommentEditorViewModelValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(150);
        }
    }
}
