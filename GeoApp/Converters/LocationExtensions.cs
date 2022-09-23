using GeoApp.Application.Model;
using GeoApp.ViewModels;

namespace GeoApp.Converters;

public static class LocationExtensions
{
    public static LocationViewModel ToViewModel(this Location location)
    {
        return new LocationViewModel(
            location.Region, 
            location.Postal, 
            location.City, 
            location.Organization, 
            location.Coordinates.ToViewModel());
    }
}