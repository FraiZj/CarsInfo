using CarsInfo.Application.BusinessLogic.External.Auth.Google.Models;
using CarsInfo.Common.Installers.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.Installers
{
    public class GoogleAuthInstaller : IInstaller
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var googleAuthSettings = new GoogleAuthSettings();
            configuration.GetSection(nameof(GoogleAuthSettings)).Bind(googleAuthSettings);
            services.AddSingleton(googleAuthSettings);
        }
    }
}