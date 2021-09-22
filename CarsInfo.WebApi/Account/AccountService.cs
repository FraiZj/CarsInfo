using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.WebApi.Account.Models;
using CarsInfo.WebApi.EmailSender;
using CarsInfo.WebApi.EmailSender.Models;

namespace CarsInfo.WebApi.Account
{
    public class AccountService : IAccountService
    {
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public AccountService(
            IEmailSender emailSender,
            ITokenService tokenService)
        {
            _emailSender = emailSender;
            _tokenService = tokenService;
        }
        
        public Task SendEmailVerificationAsync(EmailVerificationModel model)
        {
            var token = _tokenService.GenerateAccessToken(new List<Claim>
            {
                new (ClaimTypes.Email, model.Email)
            });
            var verificationLink = new Uri($"http://localhost:4200/email/verify?token={token}");
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