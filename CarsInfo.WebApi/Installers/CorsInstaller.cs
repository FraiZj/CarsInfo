using CarsInfo.Common.Installers.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.Installers
{
    public class CorsInstaller : IInstaller
    {
        public const string CarsInfoPolicy = "CarsInfoPolicy";
        
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(o => o.AddPolicy(CarsInfoPolicy, builder =>
            {
                builder.WithOrigins("http://localhost:4200/")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials();
            }));
        }
    }
}