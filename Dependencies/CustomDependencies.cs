using Dao.External;
using Dao.External._Impl;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.StarWars;
using ServiceLayer.StarWars._Impl;
using Utilities.Wrappers;
using Utilities.Wrappers._Impl;

namespace Dependencies;

public static class CustomDependencies
{

    public static void RegisterCustomServices(this IServiceCollection services)
    {
        RegisterServices(services);
        RegisterDaos(services);
        RegisterUtilities(services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        // nothing yet
        services.AddSingleton<StarWarsService, StarWarsServiceImpl>();
    }


    private static void RegisterDaos(IServiceCollection services)
    {
        services.AddSingleton<StarWarsApiDao, StarWarsApiDaoImpl>();
    }

    private static void RegisterUtilities(IServiceCollection services)
    {
        services.AddSingleton<HttpClientWrapper, HttpClientWrapperImpl>();
    }
}
