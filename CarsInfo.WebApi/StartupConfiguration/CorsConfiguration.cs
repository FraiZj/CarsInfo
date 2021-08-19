using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class CorsConfiguration
    {
        public const string CarsInfoPolicy = "CarsInfoPolicy";
        
        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(CarsInfoPolicy, builder =>
            {
                builder.WithOrigins("http://localhost:1234/")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials();
            }));
        }
    }
}
