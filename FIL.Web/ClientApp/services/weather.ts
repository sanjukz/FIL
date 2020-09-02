import axios from "axios";

var weatherapikey = "";

// get weather api key -- (1)
export function getWeather(location, cb) {
  return axios
    .get("api/weather/getconfig", {
      headers: { "Content-Type": "application/json" }
    })
    .then(function(response) {
      weatherapikey = response.data;
      GetLocationKey(location, cb);
    })
    .catch(error => console.error(error));
}

//get location key based on country/city -- (2)
export async function GetLocationKey(location, cb) {
  var countryName = location
    ? location
        .split(".")
        .join("")
        .toLowerCase()
        .replace("czechia", "Czech Republic")
    : "";
  var countryDetails, capital;
  try {
    countryDetails = await axios.get(
      `https://restcountries.eu/rest/v2/name/${countryName}?fullText=true`
    );
    capital =
      countryDetails.data &&
      countryDetails.data.length > 0 &&
      countryDetails.data[0].capital.split(".").join("").toLowerCase();
  } catch (error) {
    capital = countryName.replace("south korea","seoul");
  }
  var url = `https://dataservice.accuweather.com/locations/v1/cities/search?q=${capital}&apikey=${weatherapikey}`;
  var api_call = await fetch(url, { mode: "cors" });
  var response = api_call.json();
  response.then(async data => {
    var flag = 0;
    for (let i = 0; i < data.length; i++) {
      if (data[i].Country.EnglishName === countryName) {
        var weather = await GetWeather(data[i].Key);
        cb(weather);
        flag = 1;
        break;
      }
    }
    if (flag === 0 && data.length > 0) {
      var weather = await GetWeather(data[0].Key);
      cb(weather);
    }
  });
}

//set weather data -- (3)
export async function GetWeather(key) {
  var url = `https://dataservice.accuweather.com/forecasts/v1/daily/1day/${key}?apikey=${weatherapikey}`;
  var api_call = await fetch(url);
  var response = api_call.json();
  var weatherData = await response;
  return weatherData;
}
