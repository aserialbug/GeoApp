using GeoApp.Application.Interfaces;
using GeoApp.Application.Model;

namespace GeoApp.Application.Services;

public class CityService
{
    private readonly IRepositoryQuery _repository;

    public CityService(IRepositoryQuery repository)
    {
        _repository = repository;
    }

    public async Task<Location[]> FindLocations(City city)
    {
        return await _repository.GetLocations(city);
    }
}