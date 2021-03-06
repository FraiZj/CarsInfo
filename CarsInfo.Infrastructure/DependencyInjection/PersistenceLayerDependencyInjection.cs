using System.Data;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.Persistence.Contexts;
using CarsInfo.Infrastructure.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DependencyInjection
{
    internal static class PersistenceLayerDependencyInjection
    {
        public static void AddPersistenceLayer(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddTransient<IDbConnection, SqlConnection>(_ => new SqlConnection(connectionString));
            services.AddTransient<IDbContext, DbContext>();

            // repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICarsRepository, CarsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IGenericRepository<Comment>, CommentRepository>();
        }
    }
}
