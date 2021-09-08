using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels.Comment
{
    public class CommentEditorViewModel
    {
        [Required, MaxLength(150)]
        public string Text { get; set; }
    }
}