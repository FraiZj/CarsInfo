using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        private const string ConnectionStringName = "CarsInfoDb";
        
        public static void AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);
            services.AddDbInitialization(connectionString);
            services.AddPersistenceLayer(connectionString);
            services.AddBusinessLogicLayer();
        }
    }
}
