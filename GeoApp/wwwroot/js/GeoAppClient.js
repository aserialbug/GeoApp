// import http from 'http';

export default class GeoAppClient {

    async GetIpLocation(ipAddress) {
        let url = `${window.location.href}/ip/location?ip=${ipAddress}`;
        console.log("Calling " + url);
        let response = await fetch(url);
        if (response.ok) {
            console.log("get request - ok");
            let coordinates = response.json();
            return new GeoPoint(coordinates["latitude"], coordinates["longitude"])
        }
        else 
            alert("HTTP error: " + response.status);
    }
    
    async GetCityLocations(city) {
        let url =`${window.location.href}/city/locations?city=${city}`;
        console.log("Calling " + url);
        let response = await fetch(url);
        if (response.ok) {
            console.log("get request - ok");
            let location = response.json();
            // return new GeoPoint(coordinates["latitude"], coordinates["longitude"])
        }
        else
            alert("HTTP error: " + response.status);
    }
}