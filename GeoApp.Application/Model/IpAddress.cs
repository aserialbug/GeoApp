using System.Threading.Channels;

namespace GeoApp.Application.Model;

public class IpAddress : IEquatable<IpAddress>
{
    private readonly uint _address;

    public IpAddress(uint address)
    {
        _address = address;
    }

    public uint ToUint() => _address;

    public override string ToString()
        => $"{_address & 0xff}.{(_address & 0xff00) >> 8}.{(_address & 0xff0000) >> 16}.{(_address & 0xff000000) >> 24}";

    public byte[] GetBytes() => BitConverter.GetBytes(_address);

    public static IpAddress Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        var sectors = value.Split('.');

        if((sectors.Length != 4) ||
           !byte.TryParse(sectors[3], out var firstOctet) ||
           !byte.TryParse(sectors[2], out var secondOctet) ||
           !byte.TryParse(sectors[1], out var thirdOctet) ||
           !byte.TryParse(sectors[0], out var forthOctet))
            throw new ArgumentException($"Value {value} is not valid ip address string");
        
        uint ip = ((uint)firstOctet << 24) | ((uint)secondOctet << 16) |((uint)thirdOctet << 8) | forthOctet;

        return new IpAddress(ip);
    }

    public bool Equals(IpAddress? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _address == other._address;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IpAddress)obj);
    }

    public override int GetHashCode()
    {
        return _address.GetHashCode();
    }
}