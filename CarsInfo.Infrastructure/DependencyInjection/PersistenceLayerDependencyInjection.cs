using System.Data;
using System.Data.SqlClient;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Infrastructure.Persistence.Contexts;
using CarsInfo.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DependencyInjection
{
    internal static class PersistenceLayerDependencyInjection
    {
        public static void AddPersistenceLayer(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddTransient<IDbConnection, SqlConnection>(provider => new SqlConnection(connectionString));
            services.AddTransient<IDbContext, DbContext>();

            // repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICarsRepository, CarsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
        }
    }
}
