using CarsInfo.DAL;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using CarsInfo.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DI
{
    public static class DependenciesDAl
    {
        public static void AddDependenciesDAL(this IServiceCollection services)
        {
            services.AddTransient<IContext, JsonContext>();
            services.AddTransient<IGenericRepository<Car>, GenericRepository<Car>>();
            services.AddTransient<IGenericRepository<Brand>, GenericRepository<Brand>>();
        }
    }
}
