import axios from "axios";
import DescriptionResponseViewModel from "../models/Description/DescriptionResponseViewModel";
import CityCountryDescriptionResponseViewModel from "../models/Description/CityCountryDescriptionResponseViewModel";

function saveDescription(description) {
    return axios.post("api/cityCountryDescription/save", description, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<DescriptionResponseViewModel>)
        .catch((error) => {
        });
}

function getDescription(description) {
    return axios.post("api/get/cityCountryDescription", description, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<CityCountryDescriptionResponseViewModel>)
        .catch((error) => {
        });
}

export const descriptionService = {
    saveDescription,
    getDescription
};
