using CarsInfo.Common.Installers.Base;
using CarsInfo.WebApi.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CarsInfo.WebApi.Installers
{
    public class RedisInstaller : IInstaller
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisSettings = new RedisSettings();
            configuration.GetSection(nameof(RedisSettings)).Bind(redisSettings);
            services.AddSingleton(redisSettings);

            if (!redisSettings.Enabled)
            {
                return;
            }

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisSettings.ConnectionString));
            services.AddStackExchangeRedisCache(options => options.Configuration = redisSettings.ConnectionString);
            services.AddSingleton<IResponseCaching, ResponseCaching>();
        }
    }
}