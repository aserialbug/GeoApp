using System.Runtime.InteropServices;
using System.Text;
using GeoApp.Application.Model;

namespace GeoApp.Infrastructure.Common;

public static class LocationExtensions
{
    private const int CountryFieldOffset = 0;
    private const int CountryFieldLength = 8;
    private const int RegionFieldOffset = 8;
    private const int RegionFieldLength = 12;
    private const int PostalFieldOffset = 20;
    private const int PostalFieldLength = 12;
    private const int CityFieldOffset = 32;
    private const int CityFieldLength = 24;
    private const int OrganizationFieldOffset = 56;
    private const int OrganizationFieldLength = 32;
    private const int LatitudeFieldOffset = 88;
    private const int LongitudeFieldOffset = 92;
    
    public static Location ReadLocation(ReadOnlySpan<byte> span)
    {
        var country = Encoding.ASCII.GetString(span.Slice(CountryFieldOffset, CountryFieldLength).TrimNullBytes());
        var region = Encoding.ASCII.GetString(span.Slice(RegionFieldOffset, RegionFieldLength).TrimNullBytes());
        var postal = Encoding.ASCII.GetString(span.Slice(PostalFieldOffset, PostalFieldLength).TrimNullBytes());
        var city = Encoding.ASCII.GetString(span.Slice(CityFieldOffset, CityFieldLength).TrimNullBytes());
        var org = Encoding.ASCII.GetString(span.Slice(OrganizationFieldOffset, OrganizationFieldLength).TrimNullBytes());
        var lat = MemoryMarshal.Read<float>(span.Slice(LatitudeFieldOffset, sizeof(float)));
        var lon = MemoryMarshal.Read<float>(span.Slice(LongitudeFieldOffset, sizeof(float)));

        return new Location(country, region, postal, city, org, lat, lon);
    }
}