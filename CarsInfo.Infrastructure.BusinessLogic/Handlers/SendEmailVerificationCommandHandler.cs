﻿using System;
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
    public class SendEmailVerificationCommandHandler : IOperationResultRequestHandler<SendEmailVerificationCommand>
    {
        private readonly ApiClientOptions _apiClientOptions;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;
        private readonly ILogger<SendEmailVerificationCommandHandler> _logger;

        public SendEmailVerificationCommandHandler(
            ApiClientOptions apiClientOptions,
            IEmailSender emailSender,
            ITokenService tokenService,
            ILogger<SendEmailVerificationCommandHandler> logger)
        {
            _apiClientOptions = apiClientOptions;
            _emailSender = emailSender;
            _tokenService = tokenService;
            _logger = logger;
        }
        
        public async Task<OperationResult> Handle(
            SendEmailVerificationCommand request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var model = request.EmailBodyModel;
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
    }
}