using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.External.Auth.Google;
using CarsInfo.Application.BusinessLogic.Options;
using CarsInfo.Infrastructure.BusinessLogic.External.Auth.Google;
using CarsInfo.Infrastructure.BusinessLogic.Handlers.Base;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using CarsInfo.Infrastructure.BusinessLogic.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DependencyInjection
{
    internal static class BusinessLogicDependencyInjection
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddMediatR(typeof(IOperationResultRequestHandler<>));
            
            // Services
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFilterService, FilterService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            // Mappers
            services.AddSingleton<CarServiceMapper>();
            services.AddSingleton<BrandServiceMapper>();
            services.AddSingleton<UserServiceMapper>();
            services.AddSingleton<CommentServiceMapper>();
            services.AddSingleton<TokenServiceMapper>();

            // External services
            services.AddTransient<IGoogleAuthService, GoogleAuthService>();
        }
    }
}
