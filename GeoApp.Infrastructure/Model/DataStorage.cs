using System.Runtime.InteropServices;
using System.Text;
using GeoApp.Application.Model;
using GeoApp.Infrastructure.Common;

namespace GeoApp.Infrastructure.Model;

internal class DataStorage
{
    private DatabaseHeader _header;
    private byte[] _data;
    private int[] _sortedLocations;

    public int[] FindLocationsForCity(string cityName)
    {
        var cityBytes = GetBytes(cityName);
        var index = FindCityIndex(cityBytes);
        if (index < 0)
            throw new Exception($"City {cityName} was not found");
        
        return GetAllCityIndexes(index);
    }

    public Location GetLocationByIndex(int index)
    {
        var span = new ReadOnlySpan<byte>(_data);
        var offset = 96 * index + (int)_header.LocationArrayOffset;
        return span.Slice(offset, 96).ReadLocation();
    }

    public int? FindIpRangeIndex(IpAddress address)
    {
        var lo = 0;
        var hi = _header.RecordCount - 1;
        while (hi >= lo)
        {
            var current = lo + (hi - lo) / 2;
            var currentRange = GetRangeByIndex(current);
            var comparsion = CompareToIpAddress(address, ref currentRange);
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

        return null;
    }

    public Range GetRangeByIndex(int index)
    {
        var rangeOffset = index * 12 + (int)_header.RangesArrayOffset;
        var span = new ReadOnlySpan<byte>(_data);
        var range = MemoryMarshal.Read<Range>(span.Slice(rangeOffset, 12));
        return range;
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
    
    public ReadOnlySpan<byte> GetBytes(string city)
    {
        // В итоге должен получиться массив байт длинной 20, который содержит
        // название города и дополнен до конаца нулевыми байтами 0x00
        byte[] bytes = new byte[Constants.CityFieldLength - Constants.FieldPrefixLength];
        var cityString = city.ToString().Substring(Constants.FieldPrefixLength);
        Encoding.ASCII.GetBytes(cityString).CopyTo(bytes, 0);
        return new ReadOnlySpan<byte>(bytes);
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
    
    
    private int CompareToIpAddress(IpAddress address, ref Range range)
    {
        var uintValue = address.ToUint();

        if (uintValue < range.From)
            return -1;
        if (uintValue > range.To)
            return 1;

        return 0;
    }

    public void Initialize(byte[] data)
    {
        if (_data != null)
            throw new InvalidOperationException("Repository has already been initialized");

        _data = data ?? throw new ArgumentNullException(nameof(data));
        var span = new ReadOnlySpan<byte>(_data);
        _header = span.Slice(0, 60).ReadDatabaseHeader();
        _sortedLocations = new int[_header.RecordCount];
        for (int i = 0; i < _header.RecordCount; i++)
        {
            var iOffset = i * 4 + (int)_header.LocationsIndexOffset;
            _sortedLocations[i] = (int)_header.LocationArrayOffset + MemoryMarshal.Read<int>(span.Slice(iOffset, sizeof(int)));
        }
    }
}