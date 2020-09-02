import axios from "axios";
import { ReplayViewModel } from "../../models/CreateEventV1/ReplayViewModel";

function saveEventReplay(eventDetails) {
  return axios.post("api/save/replay-details", eventDetails, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<ReplayViewModel>)
    .catch((error) => {
    });
}

export const eventReplayService = {
  saveEventReplay,
};

