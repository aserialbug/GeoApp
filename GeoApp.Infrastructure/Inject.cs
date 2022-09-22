using GeoApp.Application.Interfaces;
using GeoApp.Infrastructure.DataLoader;
using GeoApp.Infrastructure.Model;
using GeoApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GeoApp.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddHostedService<DataLoaderHostedService>()
            .AddSingleton<DataStorage>()
            .AddSingleton<Repository>()
            .AddTransient<FileDataLoader>()
            .AddSingleton<IRepositoryQuery>(provider => provider.GetRequiredService<Repository>());

        return serviceCollection;
    }
}