import axios from "axios";

interface IGeoCodingApi {
  getLocationFromLatLong(
    lat: number,
    lon: number
  ): Promise<GeoCodingApiResponse>;
  getLatLongFromAddress(address: string): Promise<GeoCodingApiResponse>;
}

export default class GeoCodingApi implements IGeoCodingApi {
  public getLocationFromLatLong = async (
    lat: number,
    lon: number
  ): Promise<GeoCodingApiResponse> => {
    let response = await axios.get(
        `https://maps.googleapis.com/maps/api/geocode/json?latlng=${lat},${lon}&sensor=true&key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU`
    );
    let locationResult = this.googleLocationApiResultParser(response.data);
    return locationResult;
  };

  public getLatLongFromAddress = async (
    address: string
  ): Promise<GeoCodingApiResponse> => {
    let response = await axios.get(
        `https://maps.googleapis.com/maps/api/geocode/json?address=${address}&key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU`
    );
    let locationResult = this.googleLocationApiResultParser(response.data);
    locationResult.fullAddress = address;
    return locationResult;
  };

  //Api response parser to get city, state, country etc from JSON data.
  private googleLocationApiResultParser = (response): GeoCodingApiResponse => {
    var fullAddress: string = response.results[0].formatted_address,
      city: string = "",
      state: string = "",
      country: string = "";

    for (let address of response.results[0].address_components) {
      if (address.types.find(t => t == "locality")) {
        city = address.long_name;
        continue;
      }
      if (city == "" && address.types.find(t => t == "sublocality")) {
        city = address.long_name;
        continue;
      }
      if (city == "" && address.types.find(t => t == "postal_town")) {
        city = address.long_name;
        continue;
      }
      if (city == "" && address.types.find(t => t == "sublocality_level_1")) {
        city = address.long_name;
        continue;
      }
      if (city == "" && address.types.find(t => t == "sublocality_level_2")) {
        city = address.long_name;
        continue;
      }
      if (city == "" && address.types.find(t => t == "sublocality_level_3")) {
        city = address.long_name;
        continue;
      }
      if (city == "" && address.types.find(t => t == "sublocality_level_4")) {
        city = address.long_name;
        continue;
      }
      if (city == "" && address.types.find(t => t == "sublocality_level_5")) {
        city = address.long_name;
        continue;
      }
      if (
        city == "" &&
        address.types.find(t => t == "administrative_area_level_3")
      ) {
        city = address.long_name;
        continue;
      }
      if (
        state == "" &&
        address.types.find(t => t == "administrative_area_level_2")
      ) {
        state = address.long_name;
        continue;
      }
      if (address.types.find(t => t == "administrative_area_level_1")) {
        state = address.long_name;
        continue;
      }
      if (country == "" && address.types.find(t => t == "country")) {
        country = address.long_name;
        continue;
      }
    }

    let locationResult: GeoCodingApiResponse = {
      fullAddress,
      city,
      state,
      country,
      lat: response.results[0].geometry.location.lat,
      lng: response.results[0].geometry.location.lng
    };
    return locationResult;
  };
}

class GeoCodingApiResponse {
  public fullAddress: string;
  public city: string;
  public state: string;
  public country: string;
  public lat: number;
  public lng: number;
}
