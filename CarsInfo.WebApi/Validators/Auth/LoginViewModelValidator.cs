using CarsInfo.WebApi.ViewModels.Auth;
using FluentValidation;

namespace CarsInfo.WebApi.Validators.Auth
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
