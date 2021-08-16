using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Mappers;
using CarsInfo.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DI
{
    public static class DependenciesBLL
    {
        public static void AddDependenciesBLL(this IServiceCollection services)
        {
            // Services
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFilterService, FilterService>();

            // Mappers
            services.AddSingleton<CarServiceMapper>();
            services.AddSingleton<BrandServiceMapper>();
            services.AddSingleton<UserServiceMapper>();
            services.AddSingleton<CommentServiceMapper>();
        }
    }
}
