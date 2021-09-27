using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Commands;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.EmailSender;
using CarsInfo.Application.BusinessLogic.EmailSender.Models;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.BusinessLogic.Options;
using CarsInfo.Infrastructure.BusinessLogic.Handlers.Base;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.Handlers
{
    public class SendResetPasswordEmailCommandHandler : IOperationResultRequestHandler<SendResetPasswordEmailCommand>
    {
        private readonly ApiClientOptions _apiClientOptions;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public SendResetPasswordEmailCommandHandler(
            ApiClientOptions apiClientOptions,
            IEmailSender emailSender,
            ITokenService tokenService)
        {
            _apiClientOptions = apiClientOptions;
            _emailSender = emailSender;
            _tokenService = tokenService;
        }
        
        public async Task<OperationResult> Handle(
            SendResetPasswordEmailCommand request, 
            CancellationToken cancellationToken)
        {
            var model = request.EmailBodyModel;
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
    }
}