using GeoApp.Application.Exceptions;
using GeoApp.Application.Interfaces;
using GeoApp.Application.Model;
using GeoApp.Infrastructure.Model;
using Range = GeoApp.Infrastructure.Model.Range;

namespace GeoApp.Infrastructure.Repositories;

internal class Repository : IRepositoryQuery
{
    private readonly DataStorage _dataStorage;

    public Repository(DataStorage dataStorage)
    {
        _dataStorage = dataStorage;
    }

    public Task<Location[]> GetLocations(City city)
    {
        var locationIndexArray = _dataStorage.FindLocationsForCity(city.ToString());
        if(!locationIndexArray.Any())
            throw new NotFoundException($"City {city} was not found");

        var result = new Location[locationIndexArray.Length];
        for (int idx = 0; idx < locationIndexArray.Length; idx++)
        {
            result[idx] = _dataStorage.GetLocationByIndex(locationIndexArray[idx]);
        }
        
        return Task.FromResult(result);
    }
    
    public Task<GeoPoint> GetCoordinates(IpAddress address)
    {
        var ipRangeIndex = _dataStorage.FindIpRangeIndex(address);
        if (!ipRangeIndex.HasValue)
            throw new  NotFoundException($"There are no IP ranges for address {address}");

        var range = _dataStorage.GetRangeByIndex(ipRangeIndex.Value);
        var location = _dataStorage.GetLocationByIndex((int)range.LocationIndex);
        
        return Task.FromResult(new GeoPoint(location.Latitude, location.Longitude));
    }
}