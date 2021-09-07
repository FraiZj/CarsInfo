using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbInitialization(connectionString);
            services.AddPersistenceLayer(connectionString);
            services.AddBusinessLogicLayer();
        }
    }
}
