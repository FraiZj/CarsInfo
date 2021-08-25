using CarsInfo.WebApi.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.StartupConfiguration
{
    public static class ViewModelMapperDependencyInjection
    {
        public static void AddViewModelMapper(this IServiceCollection services)
        {
            services.AddSingleton<AuthorizationControllerMapper>();
            services.AddSingleton<CarsControllerMapper>();
        }
    }
}
