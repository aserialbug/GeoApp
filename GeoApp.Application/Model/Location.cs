namespace GeoApp.Application.Model;

public class Location : IEquatable<Location>
{
    public string Country { get; }
    public string Region { get; }
    public string Postal { get; }
    public string City { get; }
    public string Organization { get; }
    public float Latitude { get; }
    public float Longitude { get; }
    
    public Location(string country,
        string region,
        string postal,
        string city,
        string organization,
        float latitude,
        float longitude)
    {
        Country = country;
        Region = region;
        Postal = postal;
        City = city;
        Organization = organization;
        Latitude = latitude;
        Longitude = longitude;
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
               Latitude.Equals(other.Latitude) && 
               Longitude.Equals(other.Longitude);
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
        return HashCode.Combine(Country, Region, Postal, City, Organization, Latitude, Longitude);
    }
}