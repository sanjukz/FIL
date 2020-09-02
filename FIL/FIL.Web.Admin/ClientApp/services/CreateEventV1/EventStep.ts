import axios from "axios";
import { EventStepViewModel } from "../../models/CreateEventV1/EventStepViewModel";

function saveEventStepRequest(eventTickets) {
  return axios.post("api/save/current-step", eventTickets, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventStepViewModel>)
    .catch((error) => {
    });
}

export const EventStepService = {
  saveEventStepRequest,
};

