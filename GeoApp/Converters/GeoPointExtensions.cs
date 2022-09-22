using GeoApp.Application.Model;
using GeoApp.ViewModels;

namespace GeoApp.Converters;

public static class GeoPointExtensions
{
    public static CoordinatesViewModel ToViewModel(this GeoPoint point)
    {
        return new CoordinatesViewModel(point.Latitude, point.Longitude);
    }
}