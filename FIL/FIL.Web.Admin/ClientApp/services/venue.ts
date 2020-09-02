import axios from "axios";
import { SaveCountryResponseViewModel } from "../models/SaveCountryResponseViewModel";
import { StateResponseViewModel } from "../models/StateResponseViewModel";
import { CityResponseViewModel } from "../models/CityResponseViewModel";
import { ZipcodeResponseViewModel } from "../models/ZipcodeResponseViewModel";
import { VenueCreationResponseViewModel } from "../models/VenueCreationResponseViewModel";

function registerCoutry(countries) {
    return axios.post("api/country", countries, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<SaveCountryResponseViewModel>)
        .catch((error) => {
        });
}
function registerState(states) {
    return axios.post("api/state", states, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<StateResponseViewModel>)
        .catch((error) => {
        });
}
function registerCity(cities) {
    return axios.post("api/city", cities, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<CityResponseViewModel>)
        .catch((error) => {
        });
}
function registerZipcode(zipcodes) {
    return axios.post("api/zipcode", zipcodes, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ZipcodeResponseViewModel>)
        .catch((error) => {
        });
}

function saveVenue(venue) {
    return axios.post("api/venue", venue, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<VenueCreationResponseViewModel>)
        .catch((error) => {
        });
}

export const venueService = {
    registerCoutry,
    registerState,
    registerCity,
    registerZipcode,
    saveVenue,
};
