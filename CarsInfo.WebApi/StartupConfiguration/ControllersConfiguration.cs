using CarsInfo.WebApi.Attributes;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class ControllersConfiguration
    {
        public static void AddControllersConfiguration(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.InputFormatters.Insert(0, JsonPatchConfiguration.GetJsonPatchInputFormatter());
                    options.Filters.Add<ValidateModelAttribute>();
                })
                .AddNewtonsoftJson()
                .AddFluentValidation(
                    config => config.RegisterValidatorsFromAssemblyContaining<Startup>());
        }
    }
}
