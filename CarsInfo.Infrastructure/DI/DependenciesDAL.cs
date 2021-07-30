using System.Data;
using System.Data.SqlClient;
using CarsInfo.DAL;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DI
{
    public static class DependenciesDAL
    {
        public static void AddDependenciesDAL(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IDbConnection, SqlConnection>(provider => new SqlConnection(connectionString));
            services.AddTransient<IDbContext, DbContext>();
            services.AddTransient<IContext, JsonContext>();

            // repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICarsRepository, CarsRepository>();
        }
    }
}
