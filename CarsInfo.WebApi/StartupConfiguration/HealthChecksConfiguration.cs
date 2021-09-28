using System.Linq;
using CarsInfo.WebApi.HealthChecks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class HealthChecksConfiguration
    {
        private const string HealthCheckResponseContentType = "application/json";
        private const string HealthCheckEndpoint = "/health";
        
        public static void UseCustomHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks(HealthCheckEndpoint, new HealthCheckOptions
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
