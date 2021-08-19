using CarsInfo.Infrastructure.DependencyInjection;
using CarsInfo.WebApi.StartupConfiguration;
using CarsInfo.WebApi.StartupConfiguration.Authentication;
using CarsInfo.WebApi.StartupConfiguration.Authentication.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarsInfo.WebApi
{
    public class Startup
    {
        private const string MasterDbConnectionStringName = "MasterDb";
        private const string CarsInfoDbConnectionStringName = "CarsInfoDb";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var masterConnectionString = _configuration.GetConnectionString(MasterDbConnectionStringName);
            var connectionString = _configuration.GetConnectionString(CarsInfoDbConnectionStringName);
            var apiAuthSettings = GetApiAuthSettings(services);

            services.AddInfrastructure(masterConnectionString, connectionString);
            services.AddJwtAuthentication(apiAuthSettings);
            services.AddViewModelMapper();
            services.AddSwagger();
            services.AddCorsConfiguration();

            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, JsonPatchConfiguration.GetJsonPatchInputFormatter());
            }).AddNewtonsoftJson();
        }

        private ApiAuthSetting GetApiAuthSettings(IServiceCollection services)
        {
            var authSettingsSection = _configuration.GetSection(nameof(ApiAuthSetting));
            services.Configure<ApiAuthSetting>(authSettingsSection);

            return authSettingsSection.Get<ApiAuthSetting>();
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

            app.UseRouting();
            app.UseCors(CorsConfiguration.CarsInfoPolicy);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
