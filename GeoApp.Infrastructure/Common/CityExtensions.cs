using System.Text;
using GeoApp.Application.Model;

namespace GeoApp.Infrastructure.Common;

public static class CityExtensions
{
    public static ReadOnlySpan<byte> GetBytes(this string city)
    {
        // В итоге должен получиться массив байт длинной 20, который содержит
        // название города и дополнен до конаца нулевыми байтами 0x00
        byte[] bytes = new byte[Constants.CityFieldLength - Constants.FieldPrefixLength];
        var cityString = city.ToString().Substring(Constants.FieldPrefixLength);
        Encoding.ASCII.GetBytes(cityString).CopyTo(bytes, 0);
        return new ReadOnlySpan<byte>(bytes);
    }
}