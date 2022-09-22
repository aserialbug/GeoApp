using GeoApp.Application;
using GeoApp.Infrastructure;
using GeoApp.Middlewares;

namespace GeoApp;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddErrorHandler()
            .AddPerformanceMeasurement()
            .AddApplication()
            .AddInfrastructure()
            .AddControllers()
            .AddNewtonsoftJson();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
            .UserPerformanceMeasurement()
            .UserErrorHandler()
            .UseRouting()
            .UseStaticFiles()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}