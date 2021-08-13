using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.Mappers
{
    public static class ViewModelMapper
    {
        public static void AddViewModelMapper(this IServiceCollection services)
        {
            services.AddSingleton<AccountControllerMapper>();
            services.AddSingleton<CarsControllerMapper>();
        }
    }
}
