using CarsInfo.DAL;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DI
{
    public static class DependenciesDAL
    {
        public static void AddDependenciesDAL(this IServiceCollection services)
        {
            services.AddTransient<IContext, JsonContext>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
