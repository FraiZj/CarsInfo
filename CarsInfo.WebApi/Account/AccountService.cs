using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.WebApi.Account.Models;
using CarsInfo.WebApi.EmailSender;
using CarsInfo.WebApi.EmailSender.Models;
using Microsoft.Extensions.Logging;
using ILogger = Google.Apis.Logging.ILogger;

namespace CarsInfo.WebApi.Account
{
    public class AccountService : IAccountService
    {
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IEmailSender emailSender,
            ITokenService tokenService,
            ILogger<AccountService> logger)
        {
            _emailSender = emailSender;
            _tokenService = tokenService;
            _logger = logger;
        }
        
        public async Task<OperationResult> SendEmailVerificationAsync(EmailVerificationModel model)
        {
            try
            {
                var token = _tokenService.GenerateAccessToken(new List<Claim>
                {
                    new (ClaimTypes.Email, model.Email)
                });
                var verificationLink = new Uri($"http://localhost:4200/email/verify?token={token}");
                await _emailSender.SendEmailAsync(new EmailModel
                {
                    Email = model.Email,
                    Subject = "Email verification for CarsInfo account",
                    Message = $@"<p>Hi {model.FirstName} {model.LastName}</p>
                             <a href=""{verificationLink}"">Verify email</a>"
                });

                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while sending verification email");
                return OperationResult.ExceptionResult();
            }
        }
    }
}