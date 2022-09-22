import GeoAppClient from 'GeoAppClient';

let client = new GeoAppClient();
let point = client.GetIpLocation("252.55.163.163");
console.log(JSON.stringify(point));