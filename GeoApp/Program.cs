
namespace GeoApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await BuildWebApplication(args)
            .RunAsync();
    }

    private static IHost BuildWebApplication(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build();
    }
}