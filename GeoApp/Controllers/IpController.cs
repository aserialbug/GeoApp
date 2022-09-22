using GeoApp.Application.Model;
using GeoApp.Application.Services;
using GeoApp.Converters;
using GeoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GeoApp.Controllers;

[ApiController]
[Route("/[controller]")]
public class IpController : ControllerBase
{
    private readonly IpService _ipService;

    public IpController(IpService ipService)
    {
        _ipService = ipService;
    }
    
    /// <summary>
    /// Get user GEO coordinates by it's IP address
    /// </summary>
    /// <param name="ip">user's IP address</param>
    /// <returns></returns>
    [HttpGet("location")]
    public async Task<CoordinatesViewModel> Location([FromQuery]string ip)
    {
        var ipAddress = IpAddress.Parse(ip);
        var coordinates = await _ipService.FindCoordinates(ipAddress);
        return coordinates.ToViewModel();
    }
}