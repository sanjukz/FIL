import axios from "axios";
import { EventHostsViewModel } from "../../models/CreateEventV1/EventHostsViewModel";

function saveEventHostsRequest(eventHosts) {
  return axios.post("api/save/event-hots", eventHosts, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventHostsViewModel>)
    .catch((error) => {
    });
}

export const eventHostsService = {
  saveEventHostsRequest,
};

