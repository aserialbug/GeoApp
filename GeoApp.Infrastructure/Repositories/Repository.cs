using GeoApp.Application.Exceptions;
using GeoApp.Application.Interfaces;
using GeoApp.Application.Model;
using GeoApp.Infrastructure.Model;
using Range = GeoApp.Infrastructure.Model.Range;

namespace GeoApp.Infrastructure.Repositories;

internal class Repository : IRepositoryQuery
{
    private DatabaseHeader _header;
    private byte[] _data;
    private Range[] _rangeIndex;
    private int[] _sortedLocations;

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
        
        // var cityBytes = city.GetBytes();
        // var index = FindCityIndex(cityBytes);
        // if (index < 0)
        //     throw new Exception($"City {city} was not found");
        //
        //
        // var indexes = GetAllCityIndexes(index);
        // var span = new ReadOnlySpan<byte>(_data);
        // var result = new Location[indexes.Length];
        // for(int idx = 0; idx < indexes.Length; idx++)
        // {
        //     var locationIdx = indexes[idx];
        //     var info = LocationInfo.Read(span.Slice(_sortedLocations[locationIdx], 96));
        //     result[idx] = new Location(info.Country, info.Region, info.Postal, info.City, info.Organization,
        //         info.Latitude, info.Longitude);
        // }
        // return  Task.FromResult(result);
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

    private int[] GetAllCityIndexes(int index)
    {
        var result = new List<int> { index };
        var city = GetCityBytes(index);
        var idx = index - 1;
        while (CompareBytes(city, GetCityBytes(idx)) == 0)
        {
            result.Add(idx);
            idx--;
        }

        idx = index+ 1;
        while (CompareBytes(city, GetCityBytes(idx)) == 0)
        {
            result.Add(idx);
            idx++;
        }

        return result.ToArray();
    }

    private int BinarySearch(IpAddress ipAddress)
    {
        var lo = 0;
        var hi = _rangeIndex.Length - 1;
        while (hi >= lo)
        {
            var current = lo + (hi - lo) / 2;
            var comparsion = CompareToIpAddress(ipAddress, ref _rangeIndex[current]);
            switch (comparsion)
            {
                case 0:
                    return current;
                case > 0:
                    lo = current + 1;
                    break;
                case < 0:
                    hi = current - 1;
                    break;
            }
        }

        return -1;
    }

    private int CompareToIpAddress(IpAddress address, ref Range range)
    {
        var uintValue = address.ToUint();

        if (uintValue < range.From)
            return -1;
        if (uintValue > range.To)
            return 1;

        return 0;
    }

    private int CompareBytes(ReadOnlySpan<byte> val, ReadOnlySpan<byte> other)
    {
        var len = Math.Min(val.Length, other.Length);
        var idx = 0;
        while (idx < len)
        {
            var comparison = val[idx].CompareTo(other[idx]);
            if (comparison == 0)
                idx++;
            else
                return comparison;
        }

        return 0;
    }

    private int FindCityIndex(ReadOnlySpan<byte> city)
    {
        var span = new ReadOnlySpan<byte>(_data);
        var hi = _sortedLocations.Length - 1;
        var lo = 0;
        while (hi >= lo)
        {
            var cu = lo + (hi - lo) / 2;
            var curCity = span.Slice(_sortedLocations[cu] + 36, 20);
            var comp = CompareBytes(city, curCity);
            if (comp == 0)
                return cu;

            if (comp > 0)
                lo = cu + 1;
                
            if (comp < 0)
                hi = cu - 1;
        }
        return -1;
    }

    private ReadOnlySpan<byte> GetCityBytes(int index)
    {
        var span = new ReadOnlySpan<byte>(_data);
        var offset = _sortedLocations[index] + 36;
        return span.Slice(offset, 20);
    }
    
    private LocationInfo GetLocationById(uint id)
    {
        var lOffset = (int)(id * 96 + _header.LocationArrayOffset);
        var span = new ReadOnlySpan<byte>(_data);
        return LocationInfo.Read(span.Slice(lOffset, 96));
    }

    // public void Initialize(byte[] data)
    // {
    //     if (_data != null)
    //         throw new InvalidOperationException("Repository has already been initialized");
    //
    //     _data = data ?? throw new ArgumentNullException(nameof(data));
    //     var span = new ReadOnlySpan<byte>(_data);
    //     _header = span.Slice(0, 60).ReadDatabaseHeader();
    //     _rangeIndex = new Range[_header.RecordCount];
    //     _sortedLocations = new int[_header.RecordCount];
    //     for (int i = 0; i < _header.RecordCount; i++)
    //     {
    //         var rangeOffset = i * 12 + (int)_header.RangesArrayOffset;
    //         var iOffset = i * 4 + (int)_header.LocationsIndexOffset;
    //         _rangeIndex[i] = MemoryMarshal.Read<Range>(span.Slice(rangeOffset, 12));
    //         _sortedLocations[i] = (int)_header.LocationArrayOffset + MemoryMarshal.Read<int>(span.Slice(iOffset, 4));
    //     }
    // }
}