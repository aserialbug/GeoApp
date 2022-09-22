using GeoApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GeoApp.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<CityService>()
            .AddScoped<IpService>();
        
        return serviceCollection;
    }
}