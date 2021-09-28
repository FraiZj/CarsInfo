using CarsInfo.Application.BusinessLogic.Options;
using CarsInfo.Common.Installers.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.Installers
{
    public class ApiClientConfigurationInstaller : IInstaller
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var apiClientOptions = new ApiClientOptions();
            configuration.GetSection(nameof(ApiClientOptions)).Bind(apiClientOptions);
            services.AddSingleton(apiClientOptions);
        }
    }
}