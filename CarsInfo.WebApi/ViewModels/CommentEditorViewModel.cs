using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels
{
    public class CommentEditorViewModel
    {
        [Required, MaxLength(150)]
        public string Text { get; set; }
    }
}