using CarsInfo.WebApi.ViewModels.Brand;
using FluentValidation;

namespace CarsInfo.WebApi.Validators.Brand
{
    public class BrandEditorViewModelValidator : AbstractValidator<BrandEditorViewModel>
    {
        public BrandEditorViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
