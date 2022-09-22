namespace GeoApp.Application.Model;

public class GeoPoint : IEquatable<GeoPoint>
{
    public float Latitude { get; }
    public float Longitude { get; }
    
    public GeoPoint(float latitude, float longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public bool Equals(GeoPoint? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GeoPoint)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }
}