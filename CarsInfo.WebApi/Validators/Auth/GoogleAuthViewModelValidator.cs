using CarsInfo.WebApi.ViewModels.Auth;
using FluentValidation;

namespace CarsInfo.WebApi.Validators.Auth
{
    public class GoogleAuthViewModelValidator : AbstractValidator<GoogleAuthViewModel>
    {
        public GoogleAuthViewModelValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
