using System.Diagnostics;

namespace GeoApp.Middlewares;

internal static class InjectPerformanceMeasurement
{
    public static IServiceCollection AddPerformanceMeasurement(this IServiceCollection services)
        => services.AddScoped<PerformanceMeasurement>();
        
    public static IApplicationBuilder UserPerformanceMeasurement(this IApplicationBuilder app)
        => app.UseMiddleware<PerformanceMeasurement>();
}

public class PerformanceMeasurement : IMiddleware
{
    private readonly ILogger<PerformanceMeasurement> _logger;

    public PerformanceMeasurement(ILogger<PerformanceMeasurement> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var id = $"RequestId_{Guid.NewGuid():N}";
        var request = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        _logger.LogInformation("Start processing {Request} request {Id}", request, id);
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("Request {Id} processed in {Time} milliseconds", id, stopwatch.ElapsedMilliseconds);
        }
    }
}