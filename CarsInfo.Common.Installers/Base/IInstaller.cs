using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Common.Installers.Base
{
    public interface IInstaller
    {
        void AddServices(IServiceCollection services, IConfiguration configuration);

        int Order => -1;
    }
}