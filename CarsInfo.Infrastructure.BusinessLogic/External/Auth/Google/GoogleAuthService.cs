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

        public GoogleAuthService(
            GoogleAuthSettings googleAuthSettings)
        {
            _googleAuthSettings = googleAuthSettings;
        }

        public async Task<OperationResult<GoogleAuthResult>> AuthenticateAsync(string accessToken)
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
    }
}
