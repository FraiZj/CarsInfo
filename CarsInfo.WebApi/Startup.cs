using CarsInfo.Application.BusinessLogic.AuthModels;
using CarsInfo.Infrastructure.DependencyInjection;
using CarsInfo.WebApi.Attributes;
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
        private const string ConnectionStringName = "CarsInfoDb";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString(ConnectionStringName);
            var apiAuthSettings = GetApiAuthSettings(services);

            services.AddInfrastructure(connectionString);
            services.AddJwtAuthentication(apiAuthSettings);
            services.AddViewModelMapper();
            services.AddSwagger();
            services.AddCorsConfiguration();

            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, JsonPatchConfiguration.GetJsonPatchInputFormatter());
                options.Filters.Add(typeof(ValidateModelAttribute));
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
