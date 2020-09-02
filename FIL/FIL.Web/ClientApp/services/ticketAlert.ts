import axios from "axios";
import { fetch, addTask } from "domain-task";
import { TicketAlertUserMappingResponseViewModel } from "../models/TicketAlert/TicketAlertUserMappingResponseViewModel";

function ticketAlertRegister(user) {
    return fetch("api/ticketAlert/signup", {
        method: "post",
        body: JSON.stringify(user),
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.json() as Promise<TicketAlertUserMappingResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

export const ticketAlertservice = {
    ticketAlertRegister
};
