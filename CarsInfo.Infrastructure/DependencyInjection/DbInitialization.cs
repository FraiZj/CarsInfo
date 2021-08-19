﻿using CarsInfo.Infrastructure.DB;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Infrastructure.DependencyInjection
{
    internal static class DbInitialization
    {
        public static void AddDbInitialization(
            this IServiceCollection services, 
            string masterConnectionString, 
            string carsInfoConnectionString)
        {
            DbInitializer.Initialize(masterConnectionString, carsInfoConnectionString);
        }
    }
}
