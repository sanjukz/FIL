import axios from "axios";
import FeelUserJourneyResponseViewModel from "../models/FeelUserJourney/FeelUserJourneyResponseViewModel";

function requstDynamicSections(data) {
    return axios.post("api/dynamicsections", data, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<FeelUserJourneyResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

export const FeelUserJourneyService = {
    requstDynamicSections
};