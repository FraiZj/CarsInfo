using System.Threading.Tasks;
using CarsInfo.Infrastructure.DependencyInjection;
using CarsInfo.WebApi.StartupConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarsInfo.WebApi
{
    public class Startup
    {
        
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(_configuration);
            services.AddJwtAuthentication(_configuration);
            services.AddGoogleAuth(_configuration);
            services.AddRedisCaching(_configuration);
            services.AddEmailSenderConfiguration(_configuration);
            services.AddApiClientConfiguration(_configuration);
            services.AddViewModelMapper();
            services.AddSwagger();
            services.AddCorsConfiguration();
            services.AddHealthChecksConfiguration(_configuration);
            services.AddControllersConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfiguration.SwaggerUrl, SwaggerConfiguration.SwaggerName);
            });

            app.UseHttpsRedirection();
            app.UseCustomHealthChecks("/health");

            app.UseRouting();
            app.UseCors(CorsConfiguration.CarsInfoPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await Task.Run(() => context.Response.Redirect("/swagger"));
                });
                endpoints.MapControllers();
            });
        }
    }
}
