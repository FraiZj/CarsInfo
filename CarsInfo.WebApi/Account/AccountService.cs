using System;
using System.Threading.Tasks;
using CarsInfo.WebApi.Account.Models;
using CarsInfo.WebApi.EmailSender;
using CarsInfo.WebApi.EmailSender.Models;

namespace CarsInfo.WebApi.Account
{
    public class AccountService : IAccountService
    {
        private readonly IEmailSender _emailSender;

        public AccountService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        
        public Task SendEmailVerificationAsync(EmailVerificationModel model)
        {
            var verificationLink = new Uri($"http://localhost:4200/email/verify?email={model.Email}");
            return _emailSender.SendEmailAsync(new EmailModel
            {
                Email = model.Email,
                Subject = "Email verification for CarsInfo account",
                Message = $@"<p>Hi {model.FirstName} {model.LastName}</p>
                             <a href=""{verificationLink}"">Verify email</a>"
            });
        }
    }
}