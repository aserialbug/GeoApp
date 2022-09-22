using System.Diagnostics;
using GeoApp.Infrastructure.Model;
using GeoApp.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GeoApp.Infrastructure.DataLoader;

internal class DataLoaderHostedService : IHostedService
{
    private const string DataFileNameParameter = "DataFileName";
    
    private FileDataLoader _fileDataLoader;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataLoaderHostedService> _logger;

    public DataLoaderHostedService(FileDataLoader fileDataLoader, 
        IConfiguration configuration,
        ILogger<DataLoaderHostedService> logger)
    {
        _logger = logger;
        _fileDataLoader = fileDataLoader;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var fileName = _configuration[DataFileNameParameter];
        if (string.IsNullOrWhiteSpace(fileName))
            throw new Exception($"Configuration parameter {DataFileNameParameter} was not found");
            
        _logger.LogInformation("Start loading data...");
        var sw = Stopwatch.StartNew();

        await _fileDataLoader.LoadFromFile(AppContext.BaseDirectory + fileName, cancellationToken);
        
        sw.Stop();
        _logger.LogInformation("Data loaded in {Elapsed} milliseconds", sw.ElapsedMilliseconds);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}