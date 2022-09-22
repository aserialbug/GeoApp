namespace GeoApp.ViewModels;

public record LocationViewModel(
    string Region,
    string Postal,
    string City,
    string Organization,
    float Latitude,
    float Longitude);