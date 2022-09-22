namespace GeoApp.Middlewares;

internal static class InjectErrorHandler
{
    public static IServiceCollection AddErrorHandler(this IServiceCollection services)
        => services.AddScoped<ErrorHandler>();
        
    public static IApplicationBuilder UserErrorHandler(this IApplicationBuilder app)
        => app.UseMiddleware<ErrorHandler>();
}

internal class ErrorHandler : IMiddleware
{
    private readonly ILogger<ErrorHandler> _logger;

    public ErrorHandler(ILogger<ErrorHandler> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "error");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(exception.Message);
        }
    }
}