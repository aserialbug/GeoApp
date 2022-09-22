export default class GeoPoint {
    constructor(latitude, longitude) {
        this.lat = latitude;
        this.long = longitude;
    }
    
    get latitude() {
        return this.lat;
    }
    
    get longitude() {
        return this.long;
    }
}
