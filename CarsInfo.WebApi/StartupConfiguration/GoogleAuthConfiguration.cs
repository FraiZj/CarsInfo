using CarsInfo.Application.BusinessLogic.External.Auth.Google.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class GoogleAuthConfiguration
    {
        public static void AddGoogleAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var googleAuthSettings = new GoogleAuthSettings();
            configuration.GetSection(nameof(GoogleAuthSettings)).Bind(googleAuthSettings);
            services.AddSingleton(googleAuthSettings);
        }
    }
}
