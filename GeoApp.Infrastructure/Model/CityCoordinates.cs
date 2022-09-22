using System.Runtime.InteropServices;
using System.Text;

namespace GeoApp.Infrastructure.Model;

public readonly struct CityCoordinates
{
    public readonly string City;
    public readonly float Latitude;
    public readonly float Longitude;

    public CityCoordinates(string city, float latitude, float longitude)
    {
        City = city;
        Latitude = latitude;
        Longitude = longitude;
    }

    public static CityCoordinates Read(ReadOnlySpan<byte> span)
    {
        var city = Encoding.ASCII.GetString(span.Slice(32, 24));
        var lat = MemoryMarshal.Read<float>(span.Slice(88, sizeof(float)));
        var lon = MemoryMarshal.Read<float>(span.Slice(92, sizeof(float)));

        return new CityCoordinates(city, lat, lon);
    }
}