using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels
{
    public class BrandEditorViewModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}