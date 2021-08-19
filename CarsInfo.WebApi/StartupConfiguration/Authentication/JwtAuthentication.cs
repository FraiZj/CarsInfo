using System.Text;
using CarsInfo.WebApi.StartupConfiguration.Authentication.Factories;
using CarsInfo.WebApi.StartupConfiguration.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CarsInfo.WebApi.StartupConfiguration.Authentication
{
    public static class JwtAuthentication
    {
        public static void AddJwtAuthentication(this IServiceCollection services, ApiAuthSetting apiAuthSettings)
        {
            services.AddSingleton<ITokenFactory, JwtTokenFactory>();
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
    }
}
