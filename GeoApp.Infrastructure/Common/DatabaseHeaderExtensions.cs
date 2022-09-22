using System.Runtime.InteropServices;
using System.Text;
using GeoApp.Infrastructure.Model;

namespace GeoApp.Infrastructure.Common;

internal static class DatabaseHeaderExtensions
{
    private const int HeaderOffset = 0;
    private const int HeaderLength = 60;
    
    private const int VersionFiledOffset = 0;
    private const int NameFiledOffset = 4;
    private const int NameFiledLength = 32;
    private const int CreateAtFiledOffset = 36;
    private const int RecordCountFiledOffset = 44;
    private const int RangesArrayFiledOffset = 48;
    private const int LocationsSortedIndexFiledOffset = 52;
    private const int LocationsArrayFiledOffset = 56;
    
    public static DatabaseHeader ReadDatabaseHeader(this ReadOnlySpan<byte> header)
    {
        var version = MemoryMarshal.Read<int>(header.Slice(VersionFiledOffset, sizeof(int)));
        var name = Encoding.ASCII.GetString(header.Slice(NameFiledOffset, NameFiledLength).TrimNullBytes());
        var timestamp = MemoryMarshal.Read<ulong>(header.Slice(CreateAtFiledOffset, sizeof(ulong)));
        var records = MemoryMarshal.Read<int>(header.Slice(RecordCountFiledOffset, sizeof(int)));
        var rangesOffset = MemoryMarshal.Read<uint>(header.Slice(RangesArrayFiledOffset, sizeof(uint)));
        var locationSortedIndexOffset = MemoryMarshal.Read<uint>(header.Slice(LocationsSortedIndexFiledOffset, sizeof(uint)));
        var locationsOffset = MemoryMarshal.Read<uint>(header.Slice(LocationsArrayFiledOffset, sizeof(uint)));

        return new DatabaseHeader(version,
            name, 
            DateTime.FromBinary((long)timestamp), 
            records,
            rangesOffset,
            locationSortedIndexOffset,
            locationsOffset);
    }
}