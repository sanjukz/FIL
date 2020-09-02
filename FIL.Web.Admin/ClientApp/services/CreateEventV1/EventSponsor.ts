import axios from "axios";
import { EventSponsorViewModel } from "../../models/CreateEventV1/EventSponsorViewModel";

function saveEventSponsorRequest(eventSponsor) {
  return axios.post("api/save/event-sponsor", eventSponsor, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventSponsorViewModel>)
    .catch((error) => {
    });
}

export const eventSponsorService = {
  saveEventSponsorRequest,
};

