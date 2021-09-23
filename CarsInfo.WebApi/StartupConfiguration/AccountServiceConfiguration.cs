using Azure.Core;
using CarsInfo.WebApi.Account;
using CarsInfo.WebApi.Account.Options;
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