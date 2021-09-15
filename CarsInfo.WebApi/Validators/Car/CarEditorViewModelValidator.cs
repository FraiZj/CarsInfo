using CarsInfo.WebApi.ViewModels.Car;
using FluentValidation;

namespace CarsInfo.WebApi.Validators.Car
{
    public class CarEditorViewModelValidator : AbstractValidator<CarEditorViewModel>
    {
        public CarEditorViewModelValidator()
        {
            RuleFor(x => x.BrandId)
                .NotEmpty();

            RuleFor(x => x.Model)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .MaximumLength(150);

            RuleFor(x => x.CarPicturesUrls)
                .NotEmpty();
        }
    }
}
