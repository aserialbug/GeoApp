using System.Runtime.InteropServices;
using System.Text;
using GeoApp.Infrastructure.Common;

namespace GeoApp.Infrastructure.Model;

public readonly struct LocationInfo
{
    public string Country { get; }
    public string Region { get; }
    public string Postal { get; }
    public string City { get; }
    public string Organization { get; }
    public float Latitude { get; }
    public float Longitude { get; }
    
    private LocationInfo(string country, string region, string postal, string city, string organization, float latitude, float longitude)
    {
        Country = country;
        Region = region;
        Postal = postal;
        City = city;
        Organization = organization;
        Latitude = latitude;
        Longitude = longitude;
    }

    public static LocationInfo Read(ReadOnlySpan<byte> span)
    {
        var country = Encoding.ASCII.GetString(span.Slice(0, 8).TrimNullBytes());
        var region = Encoding.ASCII.GetString(span.Slice(8, 12).TrimNullBytes());
        var postal = Encoding.ASCII.GetString(span.Slice(20, 12).TrimNullBytes());
        var city = Encoding.ASCII.GetString(span.Slice(32, 24).TrimNullBytes());
        var org = Encoding.ASCII.GetString(span.Slice(56, 32).TrimNullBytes());
        var lat = MemoryMarshal.Read<float>(span.Slice(88, sizeof(float)));
        var lon = MemoryMarshal.Read<float>(span.Slice(92, sizeof(float)));

        return new LocationInfo(country, region, postal, city, org, lat, lon);
    }
}