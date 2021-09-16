using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.External.Auth.Google;
using CarsInfo.Application.BusinessLogic.External.Auth.Google.Models;
using CarsInfo.Application.BusinessLogic.OperationResult;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.External.Auth.Google
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleAuthSettings _googleAuthSettings;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(
            GoogleAuthSettings googleAuthSettings, 
            ILogger<GoogleAuthService> logger)
        {
            _googleAuthSettings = googleAuthSettings;
            _logger = logger;
        }

        public async Task<OperationResult<GoogleAuthResult>> AuthenticateAsync(string accessToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(accessToken, 
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new List<string>
                        {
                            _googleAuthSettings.ClientId
                        }
                    });

                var result = new GoogleAuthResult
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName
                };

                return OperationResult<GoogleAuthResult>.SuccessResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while authentication thought google api");
                return OperationResult<GoogleAuthResult>.ExceptionResult();
            }
        }
    }
}
