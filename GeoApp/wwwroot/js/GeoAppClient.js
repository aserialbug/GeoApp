import GeoPoint from "./GeoPoint.js";

export default class GeoAppClient {

    async GetIpLocation(ipAddress) {
        var path = window.location.protocol + "//" + window.location.hostname
        if(window.location.port !== undefined)
            path = path + ":" + window.location.port;
        
        let url = `${path}/ip/location?ip=${ipAddress}`;
        console.log("fetch " + url);
        let response = await fetch(url);
        if (response.ok) {
            console.log("get request - ok");
            let coordinates = await response.json();
            return new GeoPoint(coordinates["latitude"], coordinates["longitude"]);
        }
        else 
            alert("HTTP error: " + response.status);
    }
    
    async GetCityLocations(city) {
        var path = window.location.protocol + "//" + window.location.hostname
        if(window.location.port !== undefined)
            path = path + ":" + window.location.port;
        
        let url =`${path}/city/locations?city=${city}`;
        console.log("fetch " + url);
        let response = await fetch(url);
        if (response.ok) {
            console.log("get request - ok");
            let location = await response.json();
            console.log(location);
            // return new GeoPoint(coordinates["latitude"], coordinates["longitude"])
        }
        else
            alert("HTTP error: " + response.status);
    }
}