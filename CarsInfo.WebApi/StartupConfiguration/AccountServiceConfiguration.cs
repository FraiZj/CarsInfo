using Azure.Core;
using CarsInfo.WebApi.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class AccountServiceConfiguration
    {
        public static void AddAccountServiceConfiguration(this IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
        }
    }
}