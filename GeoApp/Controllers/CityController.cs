using GeoApp.Application.Model;
using GeoApp.Application.Services;
using GeoApp.Converters;
using GeoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GeoApp.Controllers;

[ApiController]
[Route("/[controller]")]
public class CityController : ControllerBase
{
    private readonly CityService _cityService;

    public CityController(CityService cityService)
    {
        _cityService = cityService;
    }
    
    [HttpGet("locations")]
    public async Task<LocationViewModel[]> Locations([FromQuery]string city)
    {
        var c = City.Parse(city);
        var locations = await _cityService.FindLocations(c);
        return locations.Select(LocationExtensions.ToViewModel).ToArray();
    }
}