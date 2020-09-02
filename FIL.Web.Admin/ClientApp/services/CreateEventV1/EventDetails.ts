import axios from "axios";
import { EventDetailViewModel } from "../../models/CreateEventV1/EventDetailViewModel";

function saveEventDetailRequest(eventDetails) {
  return axios.post("api/save/event-details", eventDetails, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventDetailViewModel>)
    .catch((error) => {
    });
}

export const eventDetailService = {
  saveEventDetailRequest,
};

