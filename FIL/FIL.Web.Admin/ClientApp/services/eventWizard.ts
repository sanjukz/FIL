import axios from "axios";
import { EventWizardResponseModel } from "../models/EventWizard/EventWizardDataViewModels";

// just a rough skeleton here
function saveEvent(event) {
    return axios.post("api/event", event, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<EventWizardResponseModel>)
        .catch((error) => {
        });
}

export const eventWizardService = {
    saveEvent,
};
