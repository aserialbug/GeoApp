using GeoApp.Infrastructure.Model;

namespace GeoApp.Infrastructure.DataLoader;

internal class FileDataLoader
{
    private readonly DataStorage _dataStorage;


    public FileDataLoader(DataStorage dataStorage)
    {
        _dataStorage = dataStorage;
    }

    public async Task LoadFromFile(string fileName, CancellationToken token)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Data file {fileName} was not found");

        
        await using var file = File.OpenRead(fileName);
        byte[] buffer = new byte[file.Length];
        _ = await file.ReadAsync(buffer, token);
        _dataStorage.Initialize(buffer);
    }
}