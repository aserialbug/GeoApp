namespace GeoApp.ViewModels;

public record LocationViewModel(
    string Region,
    string Postal,
    string City,
    string Organization,
    CoordinatesViewModel Cooordinates);