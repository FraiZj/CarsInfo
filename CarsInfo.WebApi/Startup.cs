using System.Linq;
using CarsInfo.DB;
using CarsInfo.Infrastructure.DI;
using CarsInfo.WebApi.Assistance;
using CarsInfo.WebApi.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CarsInfo.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var masterConnectionString = _configuration.GetConnectionString("MasterDb");
            var connectionString = _configuration.GetConnectionString("CarsInfoDb");
            DbInitializer.Initialize(masterConnectionString, masterConnectionString);

            services.AddDependenciesDAL(connectionString);
            services.AddDependenciesBLL();

            var apiAuthSettings = GetApiAuthSettings(services);
            services.AddJwtAuthentication(apiAuthSettings);

            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                options.Filters.Add(typeof(ValidateModelAttribute));
            }).AddNewtonsoftJson();
            services.AddSwaggerGen();
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        private ApiAuthSetting GetApiAuthSettings(IServiceCollection services)
        {
            var authSettingsSection = _configuration.GetSection(nameof(ApiAuthSetting));
            services.Configure<ApiAuthSetting>(authSettingsSection);

            return authSettingsSection.Get<ApiAuthSetting>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarsInfo API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
