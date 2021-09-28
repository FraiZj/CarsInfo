using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CarsInfo.Common.Installers.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsInfo.Common.Installers.Extensions
{
    public static class InstallerExtensions
    {
        public static void AddInstallersFromAssemblyContaining<TMarker>(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            AddInstallersFromAssembliesContaining(services, configuration, typeof(TMarker));
        }
        
        public static void AddInstallersFromAssembliesContaining(
            this IServiceCollection services, 
            IConfiguration configuration,
            params Type[] assemblyMarkers)
        {
            var assemblies = assemblyMarkers.Select(marker => marker.Assembly).ToArray();
            AddInstallersFromAssemblies(services, configuration, assemblies);
        }
        
        public static void AddInstallersFromAssemblies(
            this IServiceCollection services, 
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var installerTypes = GetInstallerTypes(assembly);
                var installers = ActivateInstallers(installerTypes);

                foreach (var installer in installers.OrderByDescending(x => x.Order))
                {
                    installer.AddServices(services, configuration);
                }
            }
        }

        private static IEnumerable<TypeInfo> GetInstallerTypes(Assembly assembly)
        {
            return assembly.DefinedTypes.Where(
                type => typeof(IInstaller).IsAssignableFrom(type) 
                        && !type.IsInterface 
                        && !type.IsAbstract);
        }

        private static IEnumerable<IInstaller> ActivateInstallers(IEnumerable<TypeInfo> installerTypes)
        {
            return installerTypes
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>();
        }
    }
}