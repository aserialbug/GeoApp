using GeoApp.Application.Model;

namespace GeoApp.Application.Interfaces;

public interface IRepositoryQuery
{
    Task<Location[]> GetLocations(City city);
    Task<GeoPoint> GetCoordinates(IpAddress address);
}