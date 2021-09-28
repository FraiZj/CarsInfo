using CarsInfo.Common.Installers.Base;
using CarsInfo.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure
{
    public class InfrastructureInstaller : IInstaller
    {
        private const string ConnectionStringName = "CarsInfoDb";
        
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);
            services.AddDbInitialization(connectionString);
            services.AddPersistenceLayer(connectionString);
            services.AddBusinessLogicLayer();
        }

        public int Order => 1;
    }
}
