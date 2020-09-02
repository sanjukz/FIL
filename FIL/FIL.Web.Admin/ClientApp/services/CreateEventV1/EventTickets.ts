import axios from "axios";
import { TicketViewModel } from "../../models/CreateEventV1/TicketViewModel";
import { DeleteTicketViewModel } from "../../models/CreateEventV1/DeleteTicketViewModel";

function saveEventTicketsRequest(eventTickets) {
  return axios.post("api/save/create-ticket", eventTickets, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<TicketViewModel>)
    .catch((error) => {
    });
}


function deleteEventTicketsRequest(eventTickets) {
  return axios.post("api/delete/event-ticket", eventTickets, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<DeleteTicketViewModel>)
    .catch((error) => {
    });
}


export const eventTicketsService = {
  saveEventTicketsRequest,
  deleteEventTicketsRequest
};

