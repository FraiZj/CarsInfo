using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels.Brand
{
    public class BrandEditorViewModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}