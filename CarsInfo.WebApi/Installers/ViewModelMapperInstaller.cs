using CarsInfo.Common.Installers.Base;
using CarsInfo.WebApi.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.WebApi.Installers
{
    public class ViewModelMapperInstaller : IInstaller
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AuthorizationControllerMapper>();
            services.AddSingleton<CarsControllerMapper>();
            services.AddSingleton<BrandControllerMapper>();
            services.AddSingleton<CommentControllerMapper>();
            services.AddSingleton<UsersControllerMapper>();
        }
    }
}