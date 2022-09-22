using System.Text;

namespace GeoApp.Application.Model;

public class City : IEquatable<City>
{
    private readonly string _city;

    private City(string city)
    {
        _city = city;
    }

    public override string ToString() => _city;

    public static City Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));
        
        if (!value.StartsWith("cit_"))
            throw new ArgumentException($"Value {value} is not valid city name");

        return new City(value);
    }

    public bool Equals(City? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _city == other._city;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((City)obj);
    }

    public override int GetHashCode()
    {
        return _city.GetHashCode();
    }
}