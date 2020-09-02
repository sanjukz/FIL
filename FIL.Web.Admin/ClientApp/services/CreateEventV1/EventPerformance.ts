import axios from "axios";
import { EventPerformanceViewModel } from "../../models/CreateEventV1/EventPerformanceViewModel";

function saveEventPerformanceRequest(eventPerformance) {
  return axios.post("api/save/event-performance", eventPerformance, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventPerformanceViewModel>)
    .catch((error) => {
    });
}

export const eventPerformanceService = {
  saveEventPerformanceRequest,
};

