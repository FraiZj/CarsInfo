using CarsInfo.Common.Installers.Base;
using CarsInfo.Infrastructure.DB.HealthChecks;
using CarsInfo.WebApi.Caching;
using CarsInfo.WebApi.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisSettings = new RedisSettings();
            configuration.GetSection(nameof(Caching.RedisSettings)).Bind(redisSettings);

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("CarsInfoDb")
                .AddRedisHealthCheck(redisSettings, "Redis");
        }
    }
    
    static class HealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddRedisHealthCheck(
            this IHealthChecksBuilder healthChecksBuilder, 
            RedisSettings redisSettings,
            string name)
        {
            return redisSettings.Enabled ? 
                healthChecksBuilder.AddCheck<RedisHealthCheck>(name) : 
                healthChecksBuilder;
        }
    }
}