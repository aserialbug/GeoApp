using System.Runtime.InteropServices;
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
        return Array.Empty<int>();
    }

    public Location GetLocationByIndex(int index)
    {
        return null;
    }

    public int? FindIpRangeIndex(IpAddress address)
    {
        return null;
    }

    public Range GetRangeByIndex(int index)
    {
        return new Range();
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