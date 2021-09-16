using System.Linq;
using CarsInfo.Infrastructure.DB.HealthChecks;
using CarsInfo.WebApi.Caching;
using CarsInfo.WebApi.HealthChecks;
using CarsInfo.WebApi.HealthChecks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class HealthChecksConfiguration
    {
        private const string HealthCheckResponseContentType = "application/json";
        private static readonly RedisSettings RedisSettings = new ();
         
        public static void AddHealthChecksConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            configuration.GetSection(nameof(Caching.RedisSettings)).Bind(RedisSettings);

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("CarsInfoDb")
                .AddRedisHealthCheck("Redis");
        }

        private static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder healthChecksBuilder, string name)
        {
            return RedisSettings.Enabled ? 
                healthChecksBuilder.AddCheck<RedisHealthCheck>(name) : 
                healthChecksBuilder;
        }
        
        public static void UseCustomHealthChecks(this IApplicationBuilder app, string path)
        {
            app.UseHealthChecks(path, new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = HealthCheckResponseContentType;

                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(entry => new HealthCheck
                        {
                            Component = entry.Key,
                            Status = entry.Value.Status.ToString(),
                            Description = entry.Value.Description
                        }),
                        Duration = report.TotalDuration
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });
        }
    }
}
