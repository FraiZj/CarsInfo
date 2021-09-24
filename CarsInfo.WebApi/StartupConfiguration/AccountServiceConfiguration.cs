using CarsInfo.Application.BusinessLogic.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class AccountServiceConfiguration
    {
        public static void AddApiClientConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var apiClientOptions = new ApiClientOptions();
            configuration.GetSection(nameof(ApiClientOptions)).Bind(apiClientOptions);
            services.AddSingleton(apiClientOptions);
        }
    }
}