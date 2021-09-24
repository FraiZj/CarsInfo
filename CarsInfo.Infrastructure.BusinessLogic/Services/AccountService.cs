using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.EmailSender;
using CarsInfo.Application.BusinessLogic.EmailSender.Models;
using CarsInfo.Application.BusinessLogic.Models;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.BusinessLogic.Options;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApiClientOptions _apiClientOptions;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            ApiClientOptions apiClientOptions,
            IEmailSender emailSender,
            ITokenService tokenService,
            ILogger<AccountService> logger)
        {
            _apiClientOptions = apiClientOptions;
            _emailSender = emailSender;
            _tokenService = tokenService;
            _logger = logger;
        }
        
        public async Task<OperationResult> SendEmailVerificationAsync(EmailBodyModel model)
        {
            try
            {
                var token = _tokenService.GenerateAccessToken(new List<Claim>
                {
                    new (ClaimTypes.Email, model.Email)
                });
                var link = new Uri($"{_apiClientOptions.BaseUrl}/email/verify?token={token}");
                await _emailSender.SendEmailAsync(new EmailModel
                {
                    Email = model.Email,
                    Subject = "Email verification for CarsInfo account",
                    Message = $@"<p>Hi {model.FirstName} {model.LastName}</p>
                             <a href=""{link}"">Verify email</a>"
                });

                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while sending verification email");
                return OperationResult.ExceptionResult();
            }
        }

        public async Task<OperationResult> SendResetPasswordEmailAsync(EmailBodyModel model)
        {
            try
            {
                var token = _tokenService.GenerateAccessToken(new List<Claim>
                {
                    new (ClaimTypes.Email, model.Email)
                });
                var link = new Uri($"{_apiClientOptions.BaseUrl}/reset-password?token={token}");
                await _emailSender.SendEmailAsync(new EmailModel
                {
                    Email = model.Email,
                    Subject = "Reset password for CarsInfo account",
                    Message = $@"<p>Hi {model.FirstName} {model.LastName}</p>
                             <a href=""{link}"">Reset password</a>"
                });

                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while sending reset password email");
                return OperationResult.ExceptionResult();
            }
        }
    }
}