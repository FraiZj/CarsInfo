using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Options;
using CarsInfo.Infrastructure.BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class AccountServiceConfiguration
    {
        public static void AddAccountServiceConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var apiClientOptions = new ApiClientOptions();
            configuration.GetSection(nameof(ApiClientOptions)).Bind(apiClientOptions);
            services.AddSingleton(apiClientOptions);
            services.AddTransient<IAccountService, AccountService>();
        }
    }
}