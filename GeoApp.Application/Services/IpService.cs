using GeoApp.Application.Interfaces;
using GeoApp.Application.Model;

namespace GeoApp.Application.Services;

public class IpService
{
    private readonly IRepositoryQuery _repository;

    public IpService(IRepositoryQuery repository)
    {
        _repository = repository;
    }

    public async Task<GeoPoint> FindCoordinates(IpAddress ip)
    {
        return await _repository.GetCoordinates(ip);
    }
}