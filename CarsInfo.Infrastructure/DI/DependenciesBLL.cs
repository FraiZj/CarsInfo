using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Services;
using CarsInfo.Infrastructure.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DI
{
    public static class DependenciesBLL
    {
        public static void AddDependenciesBLL(this IServiceCollection services)
        {
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddAutoMapper(typeof(CarMapperProfile));
        }
    }
}
