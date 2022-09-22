using System.Globalization;
using System.Runtime.InteropServices;
using GeoApp.Application.Model;

namespace GeoApp.Infrastructure.Model;

public readonly struct IpRange
{
    public IpRange(IpAddress from, IpAddress to, uint locationIndex)
    {
        From = from;
        To = to;
        LocationIndex = locationIndex;
    }

    public IpAddress From { get; }
    public IpAddress To { get; }
    public uint LocationIndex { get; }

    public static IpRange Read(ReadOnlySpan<byte> span)
    {
        var from = new IpAddress(BitConverter.ToUInt32(span.Slice(0, sizeof(uint))));
        var to = new IpAddress(BitConverter.ToUInt32(span.Slice(4, sizeof(uint))));
        var index = MemoryMarshal.Read<uint>(span.Slice(8, sizeof(uint)));//BitConverter.ToUInt32(span.Slice(8, sizeof(uint)));

        return new IpRange(from, to, index);
    }
}