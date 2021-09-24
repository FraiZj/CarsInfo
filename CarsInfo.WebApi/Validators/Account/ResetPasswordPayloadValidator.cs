using CarsInfo.WebApi.ViewModels.Account;
using FluentValidation;

namespace CarsInfo.WebApi.Validators.Account
{
    public class ResetPasswordPayloadValidator : AbstractValidator<ResetPasswordPayload>
    {
        public ResetPasswordPayloadValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}