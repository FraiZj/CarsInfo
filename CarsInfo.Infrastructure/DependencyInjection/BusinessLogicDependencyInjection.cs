﻿using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using CarsInfo.Infrastructure.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DependencyInjection
{
    internal static class BusinessLogicDependencyInjection
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
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