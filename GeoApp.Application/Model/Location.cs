namespace GeoApp.Application.Model;

public class Location : IEquatable<Location>
{
    public string Country { get; }
    public string Region { get; }
    public string Postal { get; }
    public string City { get; }
    public string Organization { get; }
    public GeoPoint Coordinates { get; }

    public Location(string country,
        string region,
        string postal,
        string city,
        string organization,
        GeoPoint coordinates)
    {
        Country = country;
        Region = region;
        Postal = postal;
        City = city;
        Organization = organization;
        Coordinates = coordinates;
    }

    public bool Equals(Location? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Country == other.Country &&
               Region == other.Region &&
               Postal == other.Postal &&
               City == other.City &&
               Organization == other.Organization &&
               Coordinates.Equals(other.Coordinates);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Location)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Country, Region, Postal, City, Organization, Coordinates);
    }
}