import GeoAppClient from "./GeoAppClient.js";

async function CallAPI() {
    let client = new GeoAppClient();
    let point = await client.GetIpLocation("252.55.163.163");
    let locations = await client.GetCityLocations("cit_A Efoc")
    console.log(point);
    console.log(locations)
}

await CallAPI();