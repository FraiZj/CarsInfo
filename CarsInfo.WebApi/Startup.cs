using System.Threading.Tasks;
using CarsInfo.Common.Installers.Extensions;
using CarsInfo.Infrastructure;
using CarsInfo.WebApi.Installers;
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
            services.AddInstallersFromAssembliesContaining(_configuration, 
                typeof(Startup), typeof(InfrastructureInstaller));
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
                c.SwaggerEndpoint(SwaggerInstaller.SwaggerJsonPath, SwaggerInstaller.SwaggerName);
            });

            app.UseHttpsRedirection();
            app.UseCustomHealthChecks();

            app.UseRouting();
            app.UseCors(CorsInstaller.CarsInfoPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await Task.Run(() => context.Response.Redirect(SwaggerInstaller.SwaggerEndpoint));
                });
                endpoints.MapControllers();
            });
        }
    }
}
