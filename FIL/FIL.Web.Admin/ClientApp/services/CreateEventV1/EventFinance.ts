import axios from "axios";
import { EventFinanceViewModel } from "../../models/CreateEventV1/EventFinanceViewModal";

function saveEventFinanceRequest(eventFinance) {
  return axios.post("api/save/event/finance", eventFinance, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventFinanceViewModel>)
    .catch((error) => {
    });
}

export const EventFinanceService = {
  saveEventFinanceRequest,
};

