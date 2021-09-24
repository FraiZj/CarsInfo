﻿using System.Text;
using CarsInfo.Application.BusinessLogic.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class JwtAuthentication
    {
        public static void AddJwtAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var apiAuthSettings = GetApiAuthSettings(services, configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(apiAuthSettings.Secret)),
                        ValidIssuer = apiAuthSettings.Issuer,
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });
            services.AddAuthorization();
        }

        private static ApiAuthOptions GetApiAuthSettings(IServiceCollection services, IConfiguration configuration)
        {
            var authSettingsSection = configuration.GetSection(nameof(ApiAuthOptions));
            services.Configure<ApiAuthOptions>(authSettingsSection);

            return authSettingsSection.Get<ApiAuthOptions>();
        }
    }
}
