using CarsInfo.WebApi.ViewModels.Auth;
using FluentValidation;

namespace CarsInfo.WebApi.Validators.Auth
{
    public class AuthViewModelValidator : AbstractValidator<AuthViewModel>
    {
        public AuthViewModelValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty();

            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }
}
